using System;
using CMPNatural.Application.Commands;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace CMPNatural.Application.Handlers.CommandHandlers.Location
{
    public class GetLocationHandler : IRequestHandler<GetLocationCompanyCommand, CommandResponse<List<LocationCompany>>>
    {
        private readonly ILocationCompanyRepository _locationRepository;

        public GetLocationHandler(ILocationCompanyRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }

        public async Task<CommandResponse<List<LocationCompany>>> Handle(GetLocationCompanyCommand request, CancellationToken cancellationToken)
        {

            var result = (await _locationRepository.GetAsync(p=>p.CompanyId==request.CompanyId)).ToList();

            return new Success<List<LocationCompany>>() { Data = result};
        }

    }
}

