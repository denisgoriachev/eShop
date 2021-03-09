using eShop.Application.Persistance;
using eShop.Application.Projections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static eShop.Domain.Events.ProductEvents.V1;

namespace eShop.Application.Features.Product.CreateProduct
{
    public class CreateProductProjectionHandler : DomainEventProjectionHandler<ProductCreated>
    {
        private readonly IApplicationDbContext _context;

        public CreateProductProjectionHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public override async Task HandleEvent(ProductCreated domainEvent, CancellationToken cancellationToken)
        {
            var product = new Persistance.Product()
            {
                Id = domainEvent.ProductId,
                VendorCode = domainEvent.VendorCode,
                Name = domainEvent.Name,
                Description = domainEvent.Description,
                CreatedAt = domainEvent.CreatedAt,
                CreatedBy = domainEvent.CreatedBy
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }
    }
}
