using eShop.Api.Product.Models;
using eShop.Application.Features.Product;
using eShop.Application.Features.Product.CreateProduct;
using eShop.Application.Features.Product.GetProduct;
using eShop.Application.Features.Product.GetProducts;
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
        public async Task<ActionResult<Guid>> Add(CreateProductModel model)
        {
            var result = await Mediator.Send(new CreateProductCommand(model.VendorCode, model.Name, model.Price, model.Currency, model.Description));

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> Get(Guid id)
        {
            var result = await Mediator.Send(new GetProductQuery(id));

            return Ok(result);
        }

        [HttpGet("all")]
        public async Task<ActionResult<ProductDto>> All()
        {
            var result = await Mediator.Send(new GetProductsQuery());

            return Ok(result);
        }
    }
}
