using System;
using CMPNatural.Core.Base;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Application.Commands.Provider;
using System.Linq;

namespace CMPNatural.Application
{
    public class ProviderGetRouteHandler : IRequestHandler<ProviderGetRouteCommand, CommandResponse<Route>>
    {
        private readonly IRouteRepository _repository;

        public ProviderGetRouteHandler(IRouteRepository _repository)
        {
            this._repository = _repository;
        }

        public async Task<CommandResponse<Route>> Handle(ProviderGetRouteCommand request, CancellationToken cancellationToken)
        {
            var result = (await _repository.GetAsync(p => p.ProviderId == request.ProviderId && p.Id == request.RouteId)).FirstOrDefault();
            return new Success<Route>() { Data = result };
        }
    }
}

