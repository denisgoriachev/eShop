using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShop.Application.Features.Product
{
    public record ProductDto
    {
        public Guid Id { get; init; }

        public string VendorCode { get; init; } = null!;

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;
    }
}
