using System;
using CMPNatural.Core.Base;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Application.Commands.Service;

namespace CMPNatural.Application.Handlers.CommandHandlers.Service
{

    public class GetProductPricePaginateHandler : IRequestHandler<GetProductPricePaginateCommand, CommandResponse<PagesQueryResponse<ProductPrice>>>
    {
        private readonly IProductPriceRepository _repository;
        public GetProductPricePaginateHandler(IProductPriceRepository repository)
        {
            _repository = repository;
        }

        public async Task<CommandResponse<PagesQueryResponse<ProductPrice>>> Handle(GetProductPricePaginateCommand request, CancellationToken cancellationToken)
        {
            var result = (await _repository.GetBasePagedAsync(request, p =>
            p.ProductId == request.ProductId &&

            (request.Enable == null || p.Enable == request.Enable)
                ));
            return new Success<PagesQueryResponse<ProductPrice>>() { Data = result };
        }
    }
}