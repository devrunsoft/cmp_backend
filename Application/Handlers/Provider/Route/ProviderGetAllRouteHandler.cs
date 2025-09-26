using System;
using CMPNatural.Core.Base;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;


namespace CMPNatural.Application
{
    public class ProviderGetAllRouteHandler : IRequestHandler<ProviderGetAllRouteCommand, CommandResponse<PagesQueryResponse<Route>>>
    {
        private readonly IRouteRepository _repository;

        public ProviderGetAllRouteHandler(IRouteRepository _repository)
        {
            this._repository = _repository;
        }

        public async Task<CommandResponse<PagesQueryResponse<Route>>> Handle(ProviderGetAllRouteCommand request, CancellationToken cancellationToken)
        {
            var result = (await _repository.GetBasePagedAsync(request, p => p.ProviderId == request.ProviderId));
            return new Success<PagesQueryResponse<Route>>() { Data = result };
        }
    }
}

