using eShop.Domain.Entities.Warehousing;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using static eShop.Domain.Events.ProductWarehouseEvents;

namespace eShop.Domain.UnitTests.Warehouse
{
    [TestFixture]
    public class CreateProductWarehouseTests
    {
        [Test, TestCaseSource(nameof(HappyTestCases))]
        public void Happy(Guid productWarehouseId, Guid productId, int quantity, string createdBy, DateTime createdAt)
        {
            var result = ProductWarehouse.Create(productWarehouseId, productId, quantity, createdBy, createdAt);

            result.Id.Should().Be(productWarehouseId);
            result.ProductId.Should().Be(productId);
            result.OnHandQuantity.Should().Be(quantity);
            result.ReservedQuantity.Should().Be(0);
            result.AvailableQuantity.Should().Be(quantity);

            result.Changes.Should().NotBeEmpty()
                .And.HaveCount(1)
                .And.ContainItemsAssignableTo<V1.ProductWarehouseCreated>();
        }

        [Test, TestCaseSource(nameof(BadTestCases))]
        public void Bad(Guid productWarehouseId, Guid productId, int quantity, string createdBy, DateTime createdAt)
        {
            Action act = () => ProductWarehouse.Create(productWarehouseId, productId, quantity, createdBy, createdAt);

            act.Should().Throw<DomainValidationException>();
        }

        static object[] HappyTestCases =
        {
            new object[] { WarehouseConstants.ProductWarehouseId, WarehouseConstants.ProductId, 0, WarehouseConstants.UserId, WarehouseConstants.Timestamp },
            new object[] { WarehouseConstants.ProductWarehouseId, WarehouseConstants.ProductId, 10, WarehouseConstants.UserId, WarehouseConstants.Timestamp },
            new object[] { WarehouseConstants.ProductWarehouseId, WarehouseConstants.ProductId, 420, WarehouseConstants.UserId, WarehouseConstants.Timestamp },
            new object[] { WarehouseConstants.ProductWarehouseId, WarehouseConstants.ProductId, 100500, WarehouseConstants.UserId, WarehouseConstants.Timestamp }
        };

        static object[] BadTestCases =
        {
            new object[] { WarehouseConstants.ProductWarehouseId, WarehouseConstants.ProductId, -1, WarehouseConstants.UserId, WarehouseConstants.Timestamp },
            new object[] { WarehouseConstants.ProductWarehouseId, WarehouseConstants.ProductId, -10, WarehouseConstants.UserId, WarehouseConstants.Timestamp },
            new object[] { WarehouseConstants.ProductWarehouseId, WarehouseConstants.ProductId, 0, "", WarehouseConstants.Timestamp },
            new object[] { WarehouseConstants.ProductWarehouseId, WarehouseConstants.ProductId, 0, null, WarehouseConstants.Timestamp },
            new object[] { WarehouseConstants.ProductWarehouseId, WarehouseConstants.ProductId, 10, "", WarehouseConstants.Timestamp },
            new object[] { WarehouseConstants.ProductWarehouseId, WarehouseConstants.ProductId, 420, null, WarehouseConstants.Timestamp }
        };
    };
}
