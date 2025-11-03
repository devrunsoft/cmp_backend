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
    public class AdminGetAllRouteHandler : IRequestHandler<AdminGetAllRouteCommand, CommandResponse<PagesQueryResponse<Route>>>
    {
        private readonly IRouteRepository _repository;

        public AdminGetAllRouteHandler(IRouteRepository _repository)
        {
            this._repository = _repository;
        }

        public async Task<CommandResponse<PagesQueryResponse<Route>>> Handle(AdminGetAllRouteCommand request, CancellationToken cancellationToken)
        {
            var result = (await _repository.GetBasePagedAsync(request));
            return new Success<PagesQueryResponse<Route>>() { Data = result };
        }
    }
}

