using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShop.Domain.Events
{
    public static class ProductWarehouseEvents
    {
        public static class V1
        {
            public record ProductWarehouseCreated(
                Guid ProductWarehouseId,
                Guid ProductId,
                int QuantityOnHand,
                string CreatedBy,
                DateTime CreatedAt
                ) : IDomainEvent;

            public record ProductReservedForOrder(
                Guid ProductWarehouseId,
                Guid OrderId,
                int ReservedQuantity,
                DateTime ReservedAt
                ) : IDomainEvent;

            public record ProductUnreservedFromOrder(
                Guid ProductWarehouseId,
                Guid OrderId,
                int UnreservedQuantity,
                DateTime UnreservedAt
                ) : IDomainEvent;

            public record ProductReceived(
                Guid ProductWarehouseId,
                int ReceivedQuantity,
                string ReceiverId,
                DateTime ReceivedAt
                ) : IDomainEvent;

            public record ProductShipped(
                Guid ProductWarehouseId,
                Guid OrderId,
                int ShippedQuantity,
                string ShipperId,
                DateTime ShippedAt
                ) : IDomainEvent;

            public record ProductOnHandQuantityAdjusted(
                Guid ProductWarehouseId,
                int QuantityAdjustment,
                string UpdatedBy,
                DateTime UpdatedAt
                ) : IDomainEvent;
        }
    }
}
