using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShop.Domain.Entities
{
    using static eShop.Domain.Events.ProductEvents;

    public class Product : Aggregate
    {
        public string SKU { get; private set; }

        public string Name { get; private set; }

        public string Description { get; private set; }

        public string CreateBy { get; private set; }

        public DateTime CreatedAt { get; private set; }

        public string? UpdatedBy { get; private set; }

        public DateTime? UpdatedAt { get; private set; }

        private Product()
        {
            SKU = null!;
            Name = null!;
            Description = null!;
            CreateBy = null!;
        }

        public static Product Create(Guid id, string sku, string name, string description, string createdBy, DateTime createdAt)
        {
            Ensure.IsNotNullOrWhiteSpace("Product SKU", sku);
            Ensure.IsNotNullOrWhiteSpace("Product Name", name);
            Ensure.IsNotNullOrWhiteSpace("Product Description", description);
            Ensure.IsNotNullOrWhiteSpace("Product Creator", createdBy);

            var product = new Product();

            product.Apply(new V1.ProductCreated(id, sku, name, description, createdBy, createdAt));

            return product;
        }

        public void UpdateInformation(string name, string description, string updatedBy, DateTime updatedAt)
        {
            Ensure.IsNotNullOrWhiteSpace("Product Name", name);
            Ensure.IsNotNullOrWhiteSpace("Product Description", description);
            Ensure.IsNotNullOrWhiteSpace("Product Updater", updatedBy);

            Apply(new V1.ProductInformationUpdated(Id, name, description, updatedBy, updatedAt));
        }

        public void ChangeSKU(string sku, string updatedBy, DateTime updatedAt)
        {
            Ensure.IsNotNullOrWhiteSpace("Product Name", sku);
            Ensure.IsNotNullOrWhiteSpace("Product Updater", updatedBy);

            Apply(new V1.ProductVendorCodeUpdated(Id, sku, updatedBy, updatedAt));
        }

        protected override void When(IDomainEvent domainEvent)
        {
            switch (domainEvent)
            {
                case V1.ProductCreated e:
                    {
                        Id = e.ProductId;
                        SKU = e.VendorCode;
                        Name = e.Name;
                        Description = e.Description;
                        CreateBy = e.CreatedBy;
                        CreatedAt = e.CreatedAt;
                        break;
                    }
                case V1.ProductVendorCodeUpdated e:
                    {
                        SKU = e.VendorCode;
                        UpdatedBy = e.UpdatedBy;
                        UpdatedAt = e.UpdatedAt;
                        break;
                    }
                case V1.ProductInformationUpdated e:
                    {
                        Name = e.VendorCode;
                        Description = e.Description;
                        UpdatedBy = e.UpdatedBy;
                        UpdatedAt = e.UpdatedAt;
                        break;
                    }
            }
        }
    }
}
