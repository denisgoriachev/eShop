using eShop.Application.Service;
using eShop.Common;
using eShop.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace eShop.Application.Features.Product.CreateProduct
{
    public record CreateProductCommand(string VendorCode, string Name, decimal Price, string Currency, string Description) : IRequest<Guid>;

    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
    {
        private readonly IAggregateStore _store;
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTimeService _dateTimeService;

        public CreateProductCommandHandler(IAggregateStore store, ICurrentUserService currentUserService, IDateTimeService dateTimeService)
        {
            _store = store;
            _currentUserService = currentUserService;
            _dateTimeService = dateTimeService;
        }

        public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var id = Guid.NewGuid();

            var product = Domain.Entities.Catalog.Product.Create(
                id,
                request.VendorCode,
                request.Name,
                request.Description,
                _currentUserService.UserId,
                _dateTimeService.Now);

            await _store.Save(product);

            return id;
        }
    }
}
