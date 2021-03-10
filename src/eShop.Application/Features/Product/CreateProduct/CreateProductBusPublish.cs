using eShop.Application.Projections;
using eShop.Application.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static eShop.Domain.Events.ProductEvents.V1;

namespace eShop.Application.Features.Product.CreateProduct
{
    public class CreateProductBusPublishHandler : DomainEventProjectionHandler<ProductCreated>
    {
        private readonly IBusPublisher _busPublisher;

        public CreateProductBusPublishHandler(IBusPublisher busPublisher)
        {
            _busPublisher = busPublisher;
        }

        public override async Task HandleEvent(ProductCreated domainEvent, CancellationToken cancellationToken)
        {
            await _busPublisher.PublishAsync("test", domainEvent);
        }
    }
}
