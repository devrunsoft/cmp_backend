using System;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Repositories;
using System.Linq;
using CMPNatural.Application.Responses.Driver;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using CMPNatural.Core.Enums;

namespace CMPNatural.Application
{

    public class DriverGetAllRouteDatesHandler : IRequestHandler<DriverGetAllRouteDatesCommand, CommandResponse<List<RouteDateResponse>>>
    {
        private readonly IRouteRepository _repository;

        public DriverGetAllRouteDatesHandler(IRouteRepository repository)
        {
            _repository = repository;
        }

        public async Task<CommandResponse<List<RouteDateResponse>>> Handle(DriverGetAllRouteDatesCommand request, CancellationToken cancellationToken)
        {
            var result = await _repository.GetAsync(
                p => p.DriverId == request.DriverId &&
                p.Date.Month == request.Date.Month &&
                p.Date.Year == request.Date.Year,
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
                .ThenInclude(x => x.Company)
            );

            var grouped = result
                .Select(gg => new RouteDateResponse()
                {
                    Id = gg.Id,
                    Name = gg.Name,
                    Date = gg.Date,
                    Routes = gg.Items
                .Select(g =>
                new RouteLocationResponse()
                {
                    Id = g.Id,
                    RouteId = gg.Id,
                    Address = g.ServiceAppointmentLocation.LocationCompany.Address,
                    PrimaryFirstName = g.ServiceAppointmentLocation.LocationCompany.PrimaryFirstName,
                    PrimaryLastName = g.ServiceAppointmentLocation.LocationCompany.PrimaryLastName,
                    PrimaryPhonNumber = g.ServiceAppointmentLocation.LocationCompany.PrimaryPhonNumber,
                    ManifestNumber = g.ManifestNumber,
                    LocationCompanyId = g.ServiceAppointmentLocation.LocationCompany.Id,
                    Lat = g.ServiceAppointmentLocation.LocationCompany.Lat,
                    Lng = g.ServiceAppointmentLocation.LocationCompany.Long,
                    CompanyName = g.ServiceAppointmentLocation.LocationCompany.Company.CompanyName,
                    Services = new RouteServices
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
                    }
                }).ToList()

                })
                .ToList();

            return new Success<List<RouteDateResponse>>()
            {
                Data = grouped
            };

        }

    }
}

