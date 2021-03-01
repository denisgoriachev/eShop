using eShop.Domain;
using EventStore.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace eShop.Infrastructure
{
    public static class ResolvedEventExtensions
    {
        public static IDomainEvent Deserialize(this ResolvedEvent resolvedEvent)
        {
            var dataType = EventTypeMap.GetType(resolvedEvent.Event.EventType);

            var data = JsonSerializer.Deserialize(resolvedEvent.Event.Data.Span, dataType) ?? throw new Exception($"Unable to read event of type {dataType?.FullName}");

            return (IDomainEvent)data;
        }
    }
}
