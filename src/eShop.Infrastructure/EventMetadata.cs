using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShop.Infrastructure
{
    public class EventMetadata
    {
        public string ClrType { get; set; }

        public string? CorrelationId { get; set; }

        public string? CausationId { get; set; }

        public EventMetadata(string clrType)
        {
            ClrType = clrType;
        }
    }
}
