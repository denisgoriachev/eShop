using eShop.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace eShop.Application.Projections
{
    public abstract class DomainEventProjectionHandler<TDomainEvent> : INotificationHandler<DomainEventProjection<TDomainEvent>>
        where TDomainEvent : IDomainEvent
    {
        public async Task Handle(DomainEventProjection<TDomainEvent> notification, CancellationToken cancellationToken)
        {
            await HandleEvent(notification.Event, cancellationToken);
        }

        public abstract Task HandleEvent(TDomainEvent domainEvent, CancellationToken cancellationToken);
    }
}
