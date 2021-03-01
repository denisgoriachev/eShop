using eShop.Domain;
using EventStore.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace eShop.Infrastructure.Services.Domain
{
    public class AggregateEventStore : IAggregateStore
    {
        private readonly EventStoreClient _client;

        public AggregateEventStore(EventStoreClient client)
        {
            _client = client;
        }

        public async Task<bool> Exists<TAggregate>(Guid id) where TAggregate : Aggregate
        {
            var stream = GetStreamName<TAggregate>(id);
            var read = _client.ReadStreamAsync(Direction.Forwards, stream, StreamPosition.Start, 1);
            var readState = await read.ReadState;

            return readState != ReadState.StreamNotFound;
        }

        public async Task<TAggregate> Load<TAggregate>(Guid id) where TAggregate : Aggregate
        {
            var stream = GetStreamName<TAggregate>(id);
            var aggregate = (TAggregate)Activator.CreateInstance(typeof(TAggregate), true)!;

            var read = _client.ReadStreamAsync(Direction.Forwards, stream, StreamPosition.Start);
            var resolvedEvents = await read.ToArrayAsync();
            var events = resolvedEvents.Select(x => x.Deserialize());

            aggregate.Load(events);

            return aggregate;
        }

        public async Task Save<TAggregate>(TAggregate aggregate) where TAggregate : Aggregate
        {
            if (aggregate == null) throw new ArgumentNullException(nameof(aggregate));

            var stream = GetStreamName<TAggregate>(aggregate.Id);
            var changes = aggregate.Changes.ToArray();
            var events = changes.Select(CreateEventData);

            var resultTask = aggregate.Version < 0
                ? _client.AppendToStreamAsync(stream, StreamState.NoStream, events)
                : _client.AppendToStreamAsync(stream, StreamRevision.FromInt64(aggregate.Version), events);

            var result = await resultTask;

            aggregate.ClearChanges();

            static EventData CreateEventData(IDomainEvent e)
            {
                var eventMetadata = new EventMetadata(e.GetType().FullName ?? throw new Exception($"Type fullname is null for domain event {e.GetType()}"))
                {
                    CorrelationId = Activity.Current?.Id ?? ""
                };

                return new EventData(
                    Uuid.NewUuid(),
                    EventTypeMap.GetTypeName(e),
                    JsonSerializer.SerializeToUtf8Bytes(e),
                    JsonSerializer.SerializeToUtf8Bytes(eventMetadata)
                );
            }
        }

        static string GetStreamName<T>(Guid entityId) => $"{typeof(T).Name}-{entityId}";
    }
}
