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
                .ThenInclude(x => x.Company)
            )).FirstOrDefault();

            result.Status = RouteStatus.InProcess;
            result.VehicleId = result.VehicleId;
            await _repository.UpdateAsync(result);

            var routeResponse = new RouteDateResponse()
            {
                Id = result.Id,
                Name = result.Name,
                Date = result.Date,
                Routes = result.Items
                .Select(g =>
                new RouteLocationResponse()
                {
                    Id = g.Id,
                    RouteId = result.Id,
                    Address = g.ServiceAppointmentLocation.LocationCompany.Address,
                    PrimaryFirstName = g.ServiceAppointmentLocation.LocationCompany.PrimaryFirstName,
                    PrimaryLastName = g.ServiceAppointmentLocation.LocationCompany.PrimaryLastName,
                    PrimaryPhonNumber = g.ServiceAppointmentLocation.LocationCompany.PrimaryPhonNumber,
                    ManifestNumber = g.ManifestNumber,
                    LocationCompanyId = g.ServiceAppointmentLocation.LocationCompany.Id,
                    Lat = g.ServiceAppointmentLocation.LocationCompany.Lat,
                    Lng = g.ServiceAppointmentLocation.LocationCompany.Long,
                    CompanyName = g.ServiceAppointmentLocation.LocationCompany.Company.CompanyName,
                    Services =new RouteServices
                    {
                        ProductName = g.ServiceAppointmentLocation.ServiceAppointment.Product.Name,
                        ProductPriceName = g.ServiceAppointmentLocation.ServiceAppointment.ProductPrice.Name,
                        IsEmegency = g.ServiceAppointmentLocation.ServiceAppointment.IsEmegency,
                        Capacity = g.ServiceAppointmentLocation.ServiceAppointment.Qty,
                        ServiceType = ((ServiceType)g.ServiceAppointmentLocation.ServiceAppointment.Product.ServiceType).GetDescription(),
                        Status = g.ServiceAppointmentLocation.Status,
                        FinishDate = g.ServiceAppointmentLocation.FinishDate,
                        StartedAt = g.ServiceAppointmentLocation.StartedAt,
                        ServiceAppointmentLocationId = g.ServiceAppointmentLocation.Id,
                        ManifestNumber = g.ManifestNumber
                    },
                }).ToList()
            };

            return new Success<RouteDateResponse>()
            {
                Data = routeResponse
            };

        }

    }
}

