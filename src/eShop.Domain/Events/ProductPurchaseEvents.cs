using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShop.Domain.Events
{
    public static class ProductPurchaseEvents
    {
        public static class V1
        {
            public record ProductPurchaseCreated(
                Guid ProductPurchaseId,
                Guid ProductId,
                decimal Cost,
                string Currency,
                string Description,
                string CreatedBy,
                DateTime CreatedAt
                ) : IDomainEvent;

            public record ProductCostIncreased(
               Guid ProductPurchaseId,
               decimal NewCost,
               string Reason,
               string UpdatedBy,
               DateTime UpdatedAt
               ) : IDomainEvent;

            public record ProductCostDecreased(
               Guid ProductPurchaseId,
               decimal NewCost,
               string Reason,
               string UpdatedBy,
               DateTime UpdatedAt
               ) : IDomainEvent;
        }
    }
}
