using eShop.Domain.ValueObjects;
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
            public record ProductCreated(
                Guid ProductId, 
                string VendorCode, 
                string Name, 
                string Description, 
                string CreatedBy, 
                DateTime CreatedAt) : IDomainEvent;

            public record ProductVendorCodeUpdated(
                Guid ProductId, 
                string VendorCode, 
                string UpdatedBy, 
                DateTime UpdatedAt) : IDomainEvent;

            public record ProductInformationUpdated(
                Guid ProductId, 
                string VendorCode, 
                string Description, 
                string UpdatedBy, 
                DateTime UpdatedAt) : IDomainEvent;

            
        }
    }
}
