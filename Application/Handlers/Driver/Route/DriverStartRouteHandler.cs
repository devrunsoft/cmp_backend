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

namespace CMPNatural.Application
{

    public class DriverStartRouteHandler : IRequestHandler<DriverStartRouteCommand, CommandResponse<RouteDateResponse>>
    {
        private readonly IRouteRepository _repository;
        private readonly IVehicleRepository _vehicleRepository;

        public DriverStartRouteHandler(IRouteRepository repository , IVehicleRepository _vehicleRepository)
        {
            _repository = repository;
            this._vehicleRepository = _vehicleRepository;
        }

        public async Task<CommandResponse<RouteDateResponse>> Handle(DriverStartRouteCommand request, CancellationToken cancellationToken)
        {
            var inProcessService = (await _repository.GetAsync(
                p => p.Status == RouteStatus.InProcess
            )).Any();

            if (inProcessService)
            {
                return new NoAcess<RouteDateResponse>
                {
                    Message = "A route is already in process. You must complete, partially completed or cancel the current route before starting a new one."
                };
            }

            var result = (await _repository.GetAsync(
                p => p.Id == request.RouteId, query=> query
                .Include(x => x.Items)
                .ThenInclude(x => x.ServiceAppointmentLocation)
                .ThenInclude(x => x.ServiceAppointment)
                .ThenInclude(x => x.Product)
                .ThenInclude(x => x.ServiceAppointment)
                .ThenInclude(x => x.ProductPrice)

                .Include(x => x.Items)
                .ThenInclude(x => x.ServiceAppointmentLocation)
                .ThenInclude(x => x.LocationCompany)
            )).FirstOrDefault();

            result.Status = RouteStatus.InProcess;
            result.VehicleId = result.VehicleId;
            await _repository.UpdateAsync(result);

            var routeResponse = new RouteDateResponse()
            {
                Id = result.Id,
                Name= result.Name,
                Routes = result.Items.Select(x=>
                new RouteLocationResponse() {
                    Id = x.Id,
                    Address = x.ServiceAppointmentLocation.LocationCompany.Address,
                    PrimaryFirstName = x.ServiceAppointmentLocation.LocationCompany.PrimaryFirstName,
                    PrimaryLastName = x.ServiceAppointmentLocation.LocationCompany.PrimaryLastName,
                    PrimaryPhonNumber = x.ServiceAppointmentLocation.LocationCompany.PrimaryPhonNumber,
                    ManifestNumber = x.ManifestNumber,
                    ProductName = x.ServiceAppointmentLocation.ServiceAppointment.Product.Name,
                    ProductPriceName = x.ServiceAppointmentLocation.ServiceAppointment.ProductPrice.Name,
                    IsEmegency = x.ServiceAppointmentLocation.ServiceAppointment.IsEmegency,
                    Capacity = x.ServiceAppointmentLocation.ServiceAppointment.Qty,
                    ServiceType = ((ServiceType)x.ServiceAppointmentLocation.ServiceAppointment.Product.ServiceType).GetDescription(),
                    Lat = x.ServiceAppointmentLocation.LocationCompany.Lat,
                    Lng = x.ServiceAppointmentLocation.LocationCompany.Long
                }).ToList()

            };

            return new Success<RouteDateResponse>()
            {
                Data = routeResponse
            };

        }

    }
}

