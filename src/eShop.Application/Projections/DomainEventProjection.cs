using eShop.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShop.Application.Projections
{
    public record DomainEventProjection<TDomainEvent> : INotification
        where TDomainEvent : IDomainEvent
    {
        public TDomainEvent Event { get; }

        public DomainEventProjection(TDomainEvent @event)
        {
            Event = @event;
        }
    }
}
