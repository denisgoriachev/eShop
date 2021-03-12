using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShop.Domain.UnitTests.Warehouse
{
    public static class WarehouseConstants
    {
        public static readonly Guid ProductWarehouseId = Guid.Parse("45A2E6A9-2107-4D25-9BB9-22FAC011415E");
        public static readonly Guid ProductId = Guid.Parse("B5617C52-1A8A-4941-8AC5-17509691D7E7");
        public static readonly DateTime Timestamp = new DateTime(2021, 3, 12, 12, 0, 0);
        public static readonly string UserId = "CB575BD1-9C15-4F0E-9A04-41CF878EED23";

        public static readonly Guid OrderId = Guid.Parse("56898032-6DF3-4FA7-AAAE-037487392DEA");
    }
}
