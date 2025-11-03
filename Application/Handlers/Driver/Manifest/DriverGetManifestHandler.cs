using System;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Entities;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using CMPNatural.Application.Mapper;
using ScoutDirect.Core.Entities.Base;

namespace CMPNatural.Application
{
    public class DriverGetManifestHandler : IRequestHandler<DriverGetManifestCommand, CommandResponse<Manifest>>
    {
        private readonly IDriverManifestRepository _repository;

        public DriverGetManifestHandler(IDriverManifestRepository _repository)
        {
            this._repository = _repository;
        }

        public async Task<CommandResponse<Manifest>> Handle(DriverGetManifestCommand request, CancellationToken cancellationToken)
        {
            var result = (await _repository.GetAsync(p => p.ManifestId == request.Id, query => query
            .Include(x => x.Manifest)
            .ThenInclude(x => x.ServiceAppointmentLocation)
            .ThenInclude(x => x.ServiceAppointment)
            .ThenInclude(x => x.ProductPrice)

            .Include(x => x.Manifest)
            .ThenInclude(x => x.ServiceAppointmentLocation)
            .ThenInclude(x => x.ServiceAppointment)
            .ThenInclude(x => x.Product)

            .Include(x => x.Manifest)
            .ThenInclude(x => x.ServiceAppointmentLocation)
            .ThenInclude(x => x.ServiceAppointment)
            .ThenInclude(x => x.ServiceAppointmentLocations)
            .ThenInclude(x => x.LocationCompany)

            .Include(x => x.Manifest)
            .ThenInclude(x => x.Request)
            .ThenInclude(x => x.Company)
            .Include(x => x.Manifest)
            .ThenInclude(x => x.Provider)
            )).FirstOrDefault().Manifest;


            return new Success<Manifest>() { Data = result };
        }
    }
}

