using System;
using CMPNatural.Core.Base;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CMPNatural.Application
{
    public class DriverGetAllManifestHandler : IRequestHandler<DriverGetAllManifestCommand, CommandResponse<PagesQueryResponse<Manifest>>>
    {
        private readonly IManifestRepository _repository;

        public DriverGetAllManifestHandler(IManifestRepository _repository)
        {
            this._repository = _repository;
        }

        public async Task<CommandResponse<PagesQueryResponse<Manifest>>> Handle(DriverGetAllManifestCommand request, CancellationToken cancellationToken)
        {
            var result = (await _repository.GetBasePagedAsync(request, p => request.DriverId == p.RouteServiceAppointmentLocation.Route.DriverId 
            && request.Status == null || p.Status == request.Status &&
            request.startDate == null || (request.startDate <= p.PreferredDate) &&
            request.endDate == null || (request.endDate >= p.PreferredDate),
                query => query
            .Include(x => x.RouteServiceAppointmentLocation)
             .ThenInclude(x => x.Route)
            .Include(x => x.Request)
            .ThenInclude(x => x.Company)
             .Include(x => x.Request)
            .ThenInclude(x => x.Provider)
            ));
            return new Success<PagesQueryResponse<Manifest>>() { Data = result };
        }
    }
}

