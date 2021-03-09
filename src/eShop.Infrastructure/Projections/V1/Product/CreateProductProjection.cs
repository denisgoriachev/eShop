using eShop.Application.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static eShop.Domain.Events.ProductEvents.V1;

namespace eShop.Infrastructure.Projections.Product
{
    public class CreateProductProjectionHandler : DomainEventProjectionHandler<CreateProduct>
    {
        private readonly IApplicationDbContext _context;

        public CreateProductProjectionHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public override async Task HandleEvent(CreateProduct domainEvent, CancellationToken cancellationToken)
        {
            var product = new Application.Persistance.Product()
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
