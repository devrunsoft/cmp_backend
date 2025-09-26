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
    public class DriverUpComingManifestHandler : IRequestHandler<DriverUpComingManifestCommand, CommandResponse<Manifest>>
    {
        private readonly IDriverManifestRepository _repository;

        public DriverUpComingManifestHandler(IDriverManifestRepository _repository)
        {
            this._repository = _repository;
        }

        public async Task<CommandResponse<Manifest>> Handle(DriverUpComingManifestCommand request, CancellationToken cancellationToken)
        {

            var result = (await _repository.GetAsync(p => p.ManifestId == 143, query => query
            .Include(x => x.Manifest)
            .ThenInclude(x => x.Invoice)
            .ThenInclude(x => x.BaseServiceAppointment)
            .ThenInclude(x => x.ProductPrice)

            .Include(x => x.Manifest)
            .ThenInclude(x => x.Invoice)
            .ThenInclude(x => x.BaseServiceAppointment)
            .ThenInclude(x => x.Product)

            .Include(x => x.Manifest)
            .ThenInclude(x => x.Invoice)
            .ThenInclude(x => x.BaseServiceAppointment)
            .ThenInclude(x => x.ServiceAppointmentLocations)
            .ThenInclude(x => x.LocationCompany)

            .Include(x => x.Manifest)
            .ThenInclude(x => x.Invoice)
            .ThenInclude(x => x.Company)
            .Include(x => x.Manifest)
            .ThenInclude(x => x.Provider)
            )).FirstOrDefault().Manifest;


            return new Success<Manifest>() { Data = result };
        }
    }
}

