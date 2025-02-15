using System;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Application.Commands.Service;
using System.Linq;

namespace CMPNatural.Application.Handlers.CommandHandlers.Service
{
    public class GetProductPriceHandler : IRequestHandler<GetProductPriceCommand, CommandResponse<List<ProductPrice>>>
    {
        private readonly IProductPriceRepository _repository;
        public GetProductPriceHandler(IProductPriceRepository repository)
        {
            _repository = repository;
        }

        public async Task<CommandResponse<List<ProductPrice>>> Handle(GetProductPriceCommand request, CancellationToken cancellationToken)
        {
            var result = (await _repository.GetAsync(p => p.Enable&& p.ProductId == request.ProductId)).ToList();
            return new Success<List<ProductPrice>>() { Data = result };
        }
    }
}

