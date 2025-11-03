using System;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Repositories;
using System.Linq;
using CMPNatural.Application.Responses.Driver;
using System.Collections.Generic;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Enums;
using Microsoft.EntityFrameworkCore;

namespace CMPNatural.Application
{

    public class DriverGetServiceAppointmentLocationHandler : IRequestHandler<DriverGetServiceAppointmentLocationCommand, CommandResponse<List<ServiceAppointmentLocation>>>
    {
        private readonly IRouteServiceAppointmentLocationRepository _repository;

        public DriverGetServiceAppointmentLocationHandler(IRouteServiceAppointmentLocationRepository repository)
        {
            _repository = repository;
        }

        public async Task<CommandResponse<List<ServiceAppointmentLocation>>> Handle(DriverGetServiceAppointmentLocationCommand request, CancellationToken cancellationToken)
        {
            var result = (await _repository.GetAsync(
                p =>
                p.Route.DriverId == request.DriverId &&
                p.Route.Id == request.RouteId &&
                p.ServiceAppointmentLocation.Status == ServiceStatus.Photo_Before_Work,
                query=>query
                .Include(x => x.Route)
                .Include(x=>x.ServiceAppointmentLocation)
                .ThenInclude(x=>x.ServiceAppointment)
                )).ToList();

            return new Success<List<ServiceAppointmentLocation>>()
            {
                Data = result.Select(x=>x.ServiceAppointmentLocation).ToList()
            };

        }

    }
}

