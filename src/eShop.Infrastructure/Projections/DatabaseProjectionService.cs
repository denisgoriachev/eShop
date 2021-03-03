using eShop.Infrastructure.Persistance;
using EventStore.Client;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace eShop.Infrastructure.Projections
{
    public class DatabaseProjectionService : IHostedService
    {
        private readonly EventStoreClient _eventStoreClient;
        private readonly string _checkpointId;
        private readonly IServiceProvider _serviceProvider;

        private StreamSubscription? _subscription;
        private Checkpoint _checkpoint = null!;

        public DatabaseProjectionService(EventStoreClient eventStoreClient,
            string checkpointId,
            IServiceProvider serviceProvider)
        {
            _eventStoreClient = eventStoreClient;
            _checkpointId = checkpointId;
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                _checkpoint = await scope.ServiceProvider.GetRequiredService<ICheckpointProvider>()
                    .GetCheckpoint(_checkpointId, cancellationToken);

                var position = _checkpoint.Position != null
                    ? new Position((ulong)_checkpoint.Position, (ulong)_checkpoint.Position)
                    : Position.Start;

                _subscription = await _eventStoreClient.SubscribeToAllAsync(
                    position,
                    HandleEvent,
                    cancellationToken: cancellationToken);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _subscription?.Dispose();
            return Task.CompletedTask;
        }

        async Task HandleEvent(StreamSubscription streamSubscription, ResolvedEvent resolvedEvent, CancellationToken cancellationToken)
        {
            if (resolvedEvent.Event.EventType.StartsWith("$")) return;

            var evt = resolvedEvent.Deserialize();

            if (evt == null)
                return;

            using (var scope = _serviceProvider.CreateScope())
            {
                var publisher = scope.ServiceProvider.GetRequiredService<IPublisher>();

                var domainProjectionType = typeof(DomainEventProjection<>).MakeGenericType(evt.GetType());
                var domainProjection = Activator.CreateInstance(domainProjectionType, evt) ?? 
                    throw new Exception($"Unable to create domain event projection for event {evt.GetType()}");

                await publisher.Publish(domainProjection, cancellationToken);

                _checkpoint.UpdatePosition(resolvedEvent.Event.Position.CommitPosition);

                await scope.ServiceProvider.GetRequiredService<ICheckpointProvider>()
                    .StoreCheckpoint(_checkpoint, cancellationToken);
            }

        }
    }
}
