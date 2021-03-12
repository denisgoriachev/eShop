using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static eShop.Domain.Events.ProductWarehouseEvents;

namespace eShop.Domain.Entities.Warehousing
{
    public class ProductWarehouse : Aggregate
    {
        public Guid ProductId { get; private set; }

        public int OnHandQuantity { get; private set; }

        public int ReservedQuantity { get; private set; }

        public int AvailableQuantity => OnHandQuantity - ReservedQuantity;

        public static ProductWarehouse Create(Guid productWarehouseId, Guid productId, int quantityOnHand, string createdBy, DateTime createdAt)
        {
            if (quantityOnHand < 0)
                throw new DomainValidationException("Quantity on hand cannot be less then zero");

            Ensure.IsNotNullOrWhiteSpace("Product Warehouse Creator", createdBy);

            var result = new ProductWarehouse();
            result.Apply(new V1.ProductWarehouseCreated(productWarehouseId, productId, quantityOnHand, createdBy, createdAt));

            return result;
        }

        public void ReserveForOrder(Guid orderId, int quantity, DateTime bookedAt)
        {
            if (quantity <= 0)
                throw new DomainValidationException("Cannot book negative or zero quantity of product");

            if (quantity > AvailableQuantity)
                throw new DomainValidationException("Insiffucient quantity of product");

            Apply(new V1.ProductReservedForOrder(Id, orderId, quantity, bookedAt));
        }

        public void UnreserveFromOrder(Guid orderId, int quantity, DateTime bookedAt)
        {
            if (quantity <= 0)
                throw new DomainValidationException("Cannot unbook negative or zero quantity of product");

            if (ReservedQuantity < quantity)
                throw new DomainValidationException("Cannot unbook quantity of products which is greater then reserved quantity");

            Apply(new V1.ProductUnreservedFromOrder(Id, orderId, quantity, bookedAt));
        }

        public void ShipProductToOrder(Guid OrderId, int quantity, string shipperId, DateTime shippedAt)
        {
            if(quantity < 0)
                throw new DomainValidationException("Cannot ship negative quantity of product");

            if(quantity > ReservedQuantity)
                throw new DomainValidationException("Cannot ship more products then it was reserved");

            if (quantity > OnHandQuantity)
                throw new DomainValidationException("Cannot ship more products then it is on-hand.");

            Apply(new V1.ProductShipped(Id, OrderId, quantity, shipperId, shippedAt));
        }

        public void ReceiveProduct(int quantity, string receiverId, DateTime receivedAt)
        {
            if (quantity < 0)
                throw new DomainValidationException("Cannot receive negative quantity of product");

            Apply(new V1.ProductReceived(Id, quantity, receiverId, receivedAt));
        }

        public void AdjustOnHandQuantity(int onHandQuantityDelta, string updatedBy, DateTime updatedAt)
        {
            Apply(new V1.ProductOnHandQuantityAdjusted(Id, onHandQuantityDelta, updatedBy, updatedAt));
        }

        protected override void When(IDomainEvent domainEvent)
        {
            switch(domainEvent)
            {
                case V1.ProductWarehouseCreated e:
                    {
                        Id = e.ProductWarehouseId;
                        ProductId = e.ProductId;
                        OnHandQuantity = e.QuantityOnHand;
                        ReservedQuantity = 0;
                        break;
                    }
                case V1.ProductReceived e:
                    {
                        OnHandQuantity += e.ReceivedQuantity;
                        break;
                    }
                case V1.ProductShipped e:
                    {
                        OnHandQuantity -= e.ShippedQuantity;
                        ReservedQuantity -= e.ShippedQuantity;
                        break;
                    }
                case V1.ProductReservedForOrder e:
                    {
                        ReservedQuantity += e.ReservedQuantity;
                        break;
                    }
                case V1.ProductUnreservedFromOrder e:
                    {
                        ReservedQuantity -= e.UnreservedQuantity;
                        break;
                    }
                case V1.ProductOnHandQuantityAdjusted e:
                    {
                        OnHandQuantity += e.QuantityAdjustment;
                        break;
                    }
            }
        }
    }
}
