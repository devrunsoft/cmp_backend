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
using CMPNatural.Core.Enums;

namespace CMPNatural.Application
{
    public class DriverStartRouteManifestHandler : IRequestHandler<DriverStartRouteManifestCommand, CommandResponse<Manifest>>
    {
        private readonly IDriverManifestRepository _repository;
        private readonly IManifestRepository _manifestRepository;

        public DriverStartRouteManifestHandler(IDriverManifestRepository _repository , IManifestRepository _manifestRepository)
        {
            this._repository = _repository;
            this._manifestRepository = _manifestRepository;
        }

        public async Task<CommandResponse<Manifest>> Handle(DriverStartRouteManifestCommand request, CancellationToken cancellationToken)
        {
            var result = (await _repository.GetAsync(p => p.ManifestId == request.ManifestId, query => query
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

            result.Status = ManifestStatus.Start_Driver;
            result.DoingStartTime = DateTime.Now;
            await _manifestRepository.UpdateAsync(result);

            return new Success<Manifest>() { Data = result };
        }
    }
}

