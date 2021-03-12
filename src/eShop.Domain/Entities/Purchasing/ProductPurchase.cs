using eShop.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShop.Domain.Entities.Purchasing
{
    using static eShop.Domain.Events.ProductPurchaseEvents;

    public class ProductPurchase : Aggregate
    {
        public Guid ProductId { get; private set; }

        public Money Cost { get; private set; }

        private ProductPurchase()
        {
            Cost = null!;
        }

        public static ProductPurchase Create(
            Guid productPurchaseId,
            Guid productId,
            decimal cost,
            string currency,
            string description,
            string createdBy,
            DateTime createdAt)
        {
            Ensure.IsNotNullOrWhiteSpace("Product Purchase Description", description);
            Ensure.IsNotNullOrWhiteSpace("Product Purchase Creator", createdBy);

            var result = new ProductPurchase();
            result.Apply(new V1.ProductPurchaseCreated(productPurchaseId, productId, cost, currency, description, createdBy, createdAt));

            return result;
        }

        public void IncreaseCost(decimal newCost, string reason, string updatedBy, DateTime updatedAt)
        {
            if (newCost <= Cost.Value)
                throw new DomainException("New cost cannot be less or equal to current.");

            Ensure.IsNotNullOrWhiteSpace("Cost Increase Reason", reason);
            Ensure.IsNotNullOrWhiteSpace("Cost Increase Updater", updatedBy);

            Apply(new V1.ProductCostIncreased(Id, newCost, reason, updatedBy, updatedAt));
        }

        public void DecreaseCost(decimal newCost, string reason, string updatedBy, DateTime updatedAt)
        {
            if (newCost >= Cost.Value)
                throw new DomainException("New cost cannot be greater or equal to current.");

            Ensure.IsNotNullOrWhiteSpace("Cost Increase Reason", reason);
            Ensure.IsNotNullOrWhiteSpace("Cost Increase Updater", updatedBy);

            Apply(new V1.ProductCostDecreased(Id, newCost, reason, updatedBy, updatedAt));
        }

        protected override void When(IDomainEvent domainEvent)
        {
            switch (domainEvent)
            {
                case V1.ProductPurchaseCreated e:
                    {
                        Id = e.ProductPurchaseId;
                        ProductId = e.ProductId;
                        Cost = new Money(e.Cost, e.Currency);
                        break;
                    }
                case V1.ProductCostIncreased e:
                    {
                        Cost = new Money(e.NewCost, Cost.Currency);
                        break;
                    }
                case V1.ProductCostDecreased e:
                    {
                        Cost = new Money(e.NewCost, Cost.Currency);
                        break;
                    }
            }
        }
    }
}
