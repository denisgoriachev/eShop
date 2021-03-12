using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShop.Domain.Events
{
    public static class ProductSaleEvents
    {
        public static class V1
        {
            public record ProductSaleCreated(
                Guid ProductSaleId,
                Guid ProductId,
                decimal Price,
                string Currency,
                string Description,
                string CreatedBy,
                DateTime CreatedAt
                ) : IDomainEvent;

            public record ProductPriceIncreased(
                Guid ProductSaleId,
                decimal NewPrice,
                string UpdatedBy,
                DateTime UpdatedAt
                ) : IDomainEvent;

            public record ProductPriceDecreased(
                Guid ProductSaleId,
                decimal NewPrice,
                string UpdatedBy,
                DateTime UpdatedAt
                ) : IDomainEvent;
        }
    }
}
