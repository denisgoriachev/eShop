using eShop.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShop.Domain.Entities.Sales
{
    using static eShop.Domain.Events.ProductSaleEvents;

    public class ProductSale : Aggregate
    {
        public Guid ProductId { get; private set; }

        public Money Price { get; private set; }

        private ProductSale()
        {
            Price = null!;
        }

        public static ProductSale Create(Guid productSaleId, Guid productId, decimal price, string currency, string description, string createdBy, DateTime createdAt)
        {
            Ensure.IsNotNullOrWhiteSpace("Product Sale Creator", createdBy);
            Ensure.IsNotNullOrWhiteSpace("Product Sale Description", description);

            var result = new ProductSale();

            result.Apply(new V1.ProductSaleCreated(productSaleId, productId, price, currency, description, createdBy, createdAt));

            return result;
        }

        public void IncreasePrice(decimal newPrice, string updatedBy, DateTime updatedAt)
        {
            if (newPrice <= Price.Value)
                throw new DomainValidationException($"New price cannot be less or equal to current price");

            Apply(new V1.ProductPriceIncreased(Id, newPrice, updatedBy, updatedAt));
        }

        public void DecreasePrice(decimal newPrice, string updatedBy, DateTime updatedAt)
        {
            if (newPrice >= Price.Value)
                throw new DomainValidationException($"New price cannot be greater or equal to current price");

            Apply(new V1.ProductPriceDecreased(Id, newPrice, updatedBy, updatedAt));
        }

        protected override void When(IDomainEvent domainEvent)
        {
            switch(domainEvent)
            {
                case V1.ProductSaleCreated e:
                    {
                        Id = e.ProductSaleId;
                        ProductId = e.ProductId;
                        Price = Money.From(e.Price, e.Currency);
                        break;
                    }
                case V1.ProductPriceIncreased e:
                    {
                        Price = Money.From(e.NewPrice, Price.Currency);
                        break;
                    }
                case V1.ProductPriceDecreased e:
                    {
                        Price = Money.From(e.NewPrice, Price.Currency);
                        break;
                    }
            }
        }
    }
}
