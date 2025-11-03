using System;
using CMPNatural.Core.Base;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using CMPNatural.Core.Enums;
using System.Linq;

namespace CMPNatural.Application
{
    public class ProviderGetAllRouteManifestHandler : IRequestHandler<ProviderGetAllRouteManifestCommand, CommandResponse<List<Manifest>>>
    {
        private readonly IManifestRepository _repository;

        public ProviderGetAllRouteManifestHandler(IManifestRepository _repository)
        {
            this._repository = _repository;
        }

        public async Task<CommandResponse<List<Manifest>>> Handle(ProviderGetAllRouteManifestCommand request, CancellationToken cancellationToken)
        {
            var result = (await _repository.GetAsync(p => p.ProviderId == request.ProviderId
             && p.Status == ManifestStatus.Assigned, query => query
            .Include(x => x.ServiceAppointmentLocation)
            .ThenInclude(x => x.ServiceAppointment)
            .ThenInclude(x => x.ProductPrice)

            .Include(x => x.ServiceAppointmentLocation)
            .ThenInclude(x => x.ServiceAppointment)
            .ThenInclude(x => x.Product)

            .Include(x => x.ServiceAppointmentLocation)
            .ThenInclude(x => x.LocationCompany)


            .Include(x => x.Request)
            .ThenInclude(x => x.Company)

            .Include(x => x.Provider)
            .Include(x => x.Request)
            .ThenInclude(x => x.Company)
            )).ToList();
            return new Success<List<Manifest>>() { Data = result };
        }
    }
}

