using eShop.Domain.Entities.Warehousing;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShop.Domain.UnitTests.Warehouse
{
    [TestFixture]
    public class ReserveForOrderTests
    {
        private ProductWarehouse Create(int initialQuantity)
        {
            return ProductWarehouse.Create(WarehouseConstants.ProductWarehouseId,
                WarehouseConstants.ProductId,
                initialQuantity,
                WarehouseConstants.UserId,
                WarehouseConstants.Timestamp);
        }

        [Test, TestCaseSource(nameof(HappyTestCases))]
        public void Happy(int initialQuantity, int reserveQuantity)
        {
            var productWarehouse = Create(initialQuantity);
            productWarehouse.ReserveForOrder(WarehouseConstants.OrderId, reserveQuantity, WarehouseConstants.Timestamp);

            productWarehouse.OnHandQuantity.Should().Be(initialQuantity);
            productWarehouse.ReservedQuantity.Should().Be(reserveQuantity);
            productWarehouse.AvailableQuantity.Should().Be(initialQuantity - reserveQuantity);
        }

        [Test, TestCaseSource(nameof(BadTestCases))]
        public void Bad(int initialQuantity, int reserveQuantity)
        {
            var productWarehouse = Create(initialQuantity);
            Action act = () => productWarehouse.ReserveForOrder(WarehouseConstants.OrderId, reserveQuantity, WarehouseConstants.Timestamp);
            act.Should().Throw<DomainValidationException>();
        }

        static object[] HappyTestCases =
        {
            new object[] { 10, 5 },
            new object[] { 10, 10 },
            new object[] { 10, 1 },
            new object[] { 10, 9 }
        };

        static object[] BadTestCases =
        {
            new object[] { 10, 11 },
            new object[] { 10, 20 },
            new object[] { 10, -10 },
            new object[] { 10, 0 }
        };
    }
}
