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
using CMPNatural.Application.Mapper;

namespace CMPNatural.Application
{

    public class DriverArrivedRouteHandler : IRequestHandler<DriverArrivedRouteCommand, CommandResponse<RouteLocationResponse>>
    {
        private readonly IRouteRepository _repository;
        private readonly IRouteServiceAppointmentLocationRepository _routeServiceAppointmentLocationRepository;

        public DriverArrivedRouteHandler(IRouteRepository repository, IRouteServiceAppointmentLocationRepository _routeServiceAppointmentLocationRepository)
        {
            _repository = repository;
            this._routeServiceAppointmentLocationRepository = _routeServiceAppointmentLocationRepository;
        }

        public async Task<CommandResponse<RouteLocationResponse>> Handle(DriverArrivedRouteCommand request, CancellationToken cancellationToken)
        {
            var result = (await _routeServiceAppointmentLocationRepository.GetAsync(
                p =>
                p.RouteId == request.RouteId &&
                 request.RouteServiceAppointmentLocationId == p.ServiceAppointmentLocationId &&
                p.Route.DriverId == request.DriverId &&
                p.Route.Status == RouteStatus.InProcess,
                query => query.Include(x => x.Route)
                .Include(x => x.ServiceAppointmentLocation)
                .ThenInclude(x => x.ServiceAppointment)
                .ThenInclude(x => x.Product)

                .Include(x => x.ServiceAppointmentLocation)
                .ThenInclude(x => x.ServiceAppointment)
                .ThenInclude(x => x.ProductPrice)

                .Include(x => x.ServiceAppointmentLocation)
                .ThenInclude(x => x.LocationCompany)
                 .ThenInclude(x => x.Company)
            )).FirstOrDefault();

            if (result == null)
            {
                return new NoAcess<RouteLocationResponse>
                {
                    Message = "No route found"
                };
            }

            result.ServiceAppointmentLocation.Status = ServiceStatus.Arrived;
            result.ServiceAppointmentLocation.ServiceAppointment.Status = ServiceStatus.Arrived;
            await _routeServiceAppointmentLocationRepository.UpdateAsync(result);

            return new Success<RouteLocationResponse>()
            {
                Data = RouteLocationMapper.Mapper.Map<RouteLocationResponse>(result)
            };

        }

    }
}

