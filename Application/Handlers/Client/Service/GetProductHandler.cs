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
    public class GetProductHandler : IRequestHandler<GetProductCommand, CommandResponse<Product>>
    {
        private readonly IProductRepository _repository;
        public GetProductHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<CommandResponse<Product>> Handle(GetProductCommand request, CancellationToken cancellationToken)
        {
            var result = (await _repository.GetAsync(p => p.Enable&& p.Id ==request.ProductId)).FirstOrDefault();
            return new Success<Product>() { Data = result };
        }
    }
}

