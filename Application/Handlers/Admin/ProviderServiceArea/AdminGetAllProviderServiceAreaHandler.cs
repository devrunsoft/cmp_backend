using System;
using CMPNatural.Core.Base;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Repositories;
using CMPNatural.Application.Commands.Admin.ProviderServiceArea;
using System.Collections.Generic;
using System.Linq;

namespace CMPNatural.Application.Handlers.Admin
{
    public class AdminGetAllProviderServiceAreaHandler : IRequestHandler<AdminGetAllProviderServiceAreaCommand, CommandResponse<List<ServiceArea>>>
    {
        private readonly IServiceAreaRepository _repository;
        public AdminGetAllProviderServiceAreaHandler(IServiceAreaRepository reposiotry)
        {
            _repository = reposiotry;
        }
        public async Task<CommandResponse<List<ServiceArea>>> Handle(AdminGetAllProviderServiceAreaCommand request, CancellationToken cancellationToken)
        {
            var result = (await _repository.GetAsync(p=>p.ProviderId == request.ProviderId)).ToList();

            return new Success<List<ServiceArea>>() { Data = result };
        }
    }
}

