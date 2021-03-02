using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShop.Api.Product.Models
{
    public record CreateProductModel(string SKU, string Name, string Description);
}
