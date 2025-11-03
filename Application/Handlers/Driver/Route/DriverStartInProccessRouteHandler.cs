using System;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Repositories;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using CMPNatural.Application.Commands.Driver.Home;
using CMPNatural.Application.Responses.Driver;
using System.Collections.Generic;
using CMPNatural.Core.Enums;
using CMPNatural.Application.Commands.Driver.Route;
using System.Text.RegularExpressions;
using CMPNatural.Core.Entities;
using CMPNatural.Application.Responses;
using CMPNatural.Application.Mapper;

namespace CMPNatural.Application
{

    public class DriverStartInProccessRouteHandler : IRequestHandler<DriverStartInProccessRouteCommand, CommandResponse<RouteLocationResponse>>
    {
        private readonly IRouteRepository _repository;
        private readonly IRouteServiceAppointmentLocationRepository _routeServiceAppointmentLocationRepository;

        public DriverStartInProccessRouteHandler(IRouteRepository repository, IRouteServiceAppointmentLocationRepository _routeServiceAppointmentLocationRepository)
        {
            _repository = repository;
            this._routeServiceAppointmentLocationRepository = _routeServiceAppointmentLocationRepository;
        }

        public async Task<CommandResponse<RouteLocationResponse>> Handle(DriverStartInProccessRouteCommand request, CancellationToken cancellationToken)
        {
            var x = (await _routeServiceAppointmentLocationRepository.GetAsync(
                p =>
                p.RouteId == request.RouteId &&
                request.RouteServiceAppointmentLocationId == p.ServiceAppointmentLocationId &&
                p.Route.DriverId == request.DriverId &&
                p.Route.Status == RouteStatus.InProcess,
                query => query.Include(x=>x.Route)
                .Include(x=>x.ServiceAppointmentLocation)
                .ThenInclude(x=>x.ServiceAppointment)
                .ThenInclude(x => x.Product)

                .Include(x => x.ServiceAppointmentLocation)
                .ThenInclude(x => x.ServiceAppointment)
                .ThenInclude(x => x.ProductPrice)

                .Include(x => x.ServiceAppointmentLocation)
                .ThenInclude(x => x.LocationCompany)
                 .ThenInclude(x => x.Company)
            )).FirstOrDefault();

            if (x== null)
            {
                return new NoAcess<RouteLocationResponse>
                {
                    Message = "No route found for the specified driver. Please verify the route ID or assign the driver to this route before starting."
                };
            }

                x.ServiceAppointmentLocation.Status = ServiceStatus.In_Process;
                x.ServiceAppointmentLocation.ServiceAppointment.Status = ServiceStatus.In_Process;


            await _routeServiceAppointmentLocationRepository.UpdateAsync(x);

            return new Success<RouteLocationResponse>()
            {
                Data = RouteLocationMapper.Mapper.Map<RouteLocationResponse>(x)
            };

        }

    }
}

