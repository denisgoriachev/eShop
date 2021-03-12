using eShop.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShop.Domain.Entities.Catalog
{
    using static eShop.Domain.Events.ProductEvents;

    public class Product : Aggregate
    {
        public string VendorCode { get; private set; }

        public string Name { get; private set; }

        public string Description { get; private set; }

        private Product()
        {
            VendorCode = null!;
            Name = null!;
            Description = null!;
        }

        public static Product Create(Guid id, string vendorCode, string name, string description, string createdBy, DateTime createdAt)
        {
            Ensure.IsNotNullOrWhiteSpace("Product Vendor Code", vendorCode);
            Ensure.IsNotNullOrWhiteSpace("Product Name", name);
            Ensure.IsNotNullOrWhiteSpace("Product Description", description);
            Ensure.IsNotNullOrWhiteSpace("Product Creator", createdBy);

            var product = new Product();

            product.Apply(new V1.ProductCreated(id, vendorCode, name, description,  createdBy, createdAt));

            return product;
        }

        public void UpdateInformation(string name, string description, string updatedBy, DateTime updatedAt)
        {
            Ensure.IsNotNullOrWhiteSpace("Product Name", name);
            Ensure.IsNotNullOrWhiteSpace("Product Description", description);
            Ensure.IsNotNullOrWhiteSpace("Product Updater", updatedBy);

            Apply(new V1.ProductInformationUpdated(Id, name, description, updatedBy, updatedAt));
        }

        public void ChangeVendorCode(string vendorCode, string updatedBy, DateTime updatedAt)
        {
            Ensure.IsNotNullOrWhiteSpace("Product Vendor Code", vendorCode);
            Ensure.IsNotNullOrWhiteSpace("Product Updater", updatedBy);

            Apply(new V1.ProductVendorCodeUpdated(Id, vendorCode, updatedBy, updatedAt));
        }

        protected override void When(IDomainEvent domainEvent)
        {
            switch (domainEvent)
            {
                case V1.ProductCreated e:
                    {
                        Id = e.ProductId;
                        VendorCode = e.VendorCode;
                        Name = e.Name;
                        Description = e.Description;
                        break;
                    }
                case V1.ProductVendorCodeUpdated e:
                    {
                        VendorCode = e.VendorCode;
                        break;
                    }
                case V1.ProductInformationUpdated e:
                    {
                        Name = e.VendorCode;
                        Description = e.Description;
                        break;
                    }
            }
        }
    }
}
