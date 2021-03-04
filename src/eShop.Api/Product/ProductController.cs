using eShop.Api.Product.Models;
using eShop.Application.Product.CreateProduct;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShop.Api.Product
{
    [Authorize]
    public class ProductController : AbstractApiController
    {
        [HttpPost]
        public async Task<Guid> Add(CreateProductModel model)
        {
            var result = await Mediator.Send(new CreateProductCommand(model.SKU, model.Name, model.Description));
            
            return result;
        }
    }
}
