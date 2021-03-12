using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShop.Api.Product.Models
{
    public record CreateProductModel(string VendorCode, string Name, string Description, decimal Price, string Currency);
}
