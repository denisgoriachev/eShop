using eShop.Application.Persistance;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace eShop.Application.Features.Product.GetProducts
{
    public record GetProductsQuery() : IRequest<IEnumerable<ProductDto>>;

    public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, IEnumerable<ProductDto>>
    {
        private readonly IApplicationDbContext _context;

        public GetProductsQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public Task<IEnumerable<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            var result = _context.Products
                .Select(e => new ProductDto()
                {
                    Id = e.Id,
                    Name = e.Name,
                    VendorCode = e.VendorCode,
                    Description = e.Description
                }).AsEnumerable();

            return Task.FromResult(result);
        }
    }
}
