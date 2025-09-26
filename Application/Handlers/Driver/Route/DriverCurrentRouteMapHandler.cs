using System;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Repositories;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using CMPNatural.Application.Responses.Driver;
using CMPNatural.Core.Enums;
using CMPNatural.Application.Commands.Driver.Route;

namespace CMPNatural.Application
{

    public class DriverCurrentRouteMapHandler : IRequestHandler<DriverCurrentRouteMapCommand, CommandResponse<RouteDateResponse>>
    {
        private readonly IRouteRepository _repository;


        public DriverCurrentRouteMapHandler(IRouteRepository repository)
        {
            _repository = repository;
        }

        public async Task<CommandResponse<RouteDateResponse>> Handle(DriverCurrentRouteMapCommand request, CancellationToken cancellationToken)
        {
            var result = (await _repository.GetAsync(
                p => p.Status == RouteStatus.InProcess,
                query => query
                .Include(x => x.Items)
                .ThenInclude(x => x.ServiceAppointmentLocation)
                .ThenInclude(x => x.ServiceAppointment)
                .ThenInclude(x => x.Product)
                .ThenInclude(x => x.ServiceAppointment)
                .ThenInclude(x => x.ProductPrice)

                .Include(x => x.Items)
                .ThenInclude(x => x.ServiceAppointmentLocation)
                .ThenInclude(x => x.LocationCompany)
            )).LastOrDefault();

            if (result == null)
            {
                return new NoAcess<RouteDateResponse>
                {
                    Message = "No active route was found. Please start a new route."
                };
            }

            var routeResponse = new RouteDateResponse()
            {
                Id = result.Id,
                Name = result.Name,
                Routes = result.Items.Select(x =>
                new RouteLocationResponse()
                {
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

