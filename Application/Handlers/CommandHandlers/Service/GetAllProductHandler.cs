using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace CMPNatural.Application.Handlers.CommandHandlers.Service
{

    public class GetAllProductHandler : IRequestHandler<GetAllProductCommand, CommandResponse<List<Product>>>
    {
        private readonly IProductRepository _repository;
        public GetAllProductHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<CommandResponse<List<Product>>> Handle(GetAllProductCommand request, CancellationToken cancellationToken)
        {
            var result = (await _repository.GetAsync(p => p.Enable)).OrderBy(x=>x.Order).ToList();
            return new Success<List<Product>>() { Data = result };
        }
    }
}

