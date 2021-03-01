using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShop.Domain.Events
{
    public static class ProductEvents
    {
        public static class V1
        {
            public record CreateProduct(Guid ProductId, string SKU, string Name, string Description, string CreatedBy, DateTime CreatedAt) : IDomainEvent;

            public record UpdateProductSKU(Guid ProductId, string SKU, string UpdatedBy, DateTime UpdatedAt) : IDomainEvent;

            public record UpdateProductInformation(Guid ProductId, string Name, string Description, string UpdatedBy, DateTime UpdatedAt) : IDomainEvent;
        }
    }
}
