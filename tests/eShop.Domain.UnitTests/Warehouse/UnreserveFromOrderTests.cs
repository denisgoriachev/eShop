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
    public class UnreserveFromOrderTests
    {
        private ProductWarehouse Create(int initialQuantity, int reserveQuantity)
        {
            var result = ProductWarehouse.Create(WarehouseConstants.ProductWarehouseId,
                WarehouseConstants.ProductId,
                initialQuantity,
                WarehouseConstants.UserId,
                WarehouseConstants.Timestamp);

            result.ReserveForOrder(WarehouseConstants.OrderId, reserveQuantity, WarehouseConstants.Timestamp);

            return result;
        }

        [Test, TestCaseSource(nameof(HappyTestCases))]
        public void Happy(int initialQuantity, int reserveQuantity, int unreserveQuantity)
        {
            var productWarehouse = Create(initialQuantity, reserveQuantity);
            productWarehouse.UnreserveFromOrder(WarehouseConstants.OrderId, unreserveQuantity, WarehouseConstants.Timestamp);

            productWarehouse.OnHandQuantity.Should().Be(initialQuantity);
            productWarehouse.ReservedQuantity.Should().Be(reserveQuantity - unreserveQuantity);
        }

        [Test, TestCaseSource(nameof(BadTestCases))]
        public void Bad(int initialQuantity, int reserveQuantity, int unreserveQuantity)
        {
            var productWarehouse = Create(initialQuantity, reserveQuantity);
            Action act = () => productWarehouse.UnreserveFromOrder(WarehouseConstants.OrderId, unreserveQuantity, WarehouseConstants.Timestamp);
            act.Should().Throw<DomainValidationException>();
        }

        static object[] HappyTestCases =
        {
            new object[] { 10, 5, 5 },
            new object[] { 10, 10, 5 },
            new object[] { 10, 1, 1 },
            new object[] { 10, 9, 8 }
        };

        static object[] BadTestCases =
        {
            new object[] { 10, 5, 6 },
            new object[] { 10, 5, 10 },
            new object[] { 10, 5, -1 },
            new object[] { 10, 5, 0 }
        };
    }
}
