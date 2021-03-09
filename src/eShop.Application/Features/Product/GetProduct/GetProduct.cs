using eShop.Application.Exceptions;
using eShop.Application.Persistance;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace eShop.Application.Features.Product.GetProduct
{
    public record GetProductQuery(Guid ProductId) : IRequest<ProductDto>;

    public class GetProductQueryHandler : IRequestHandler<GetProductQuery, ProductDto>
    {
        private readonly IApplicationDbContext _context;

        public GetProductQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ProductDto> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            var productDtos = _context.Products.Where(e => e.Id == request.ProductId)
                .Select(e => new ProductDto()
                {
                    Id = e.Id,
                    Name = e.Name,
                    VendorCode = e.VendorCode,
                    Description = e.Description
                });

            var result = await productDtos.FirstOrDefaultAsync();

            if (result == null)
                throw new NotFoundException(nameof(Product), request.ProductId);

            return result;
        }
    }
}
