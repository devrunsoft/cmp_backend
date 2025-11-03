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
using CMPNatural.Application.Responses.Service;
using System.Net.Http;
using System.Collections.Generic;
using CMPNatural.Application.Responses;
using GeoCoordinatePortable;
using CMPNatural.Core.Entities;

namespace CMPNatural.Application
{

    public class AdminGetRouteHandler : IRequestHandler<AdminGetRouteCommand, CommandResponse<RouteDateResponse>>
    {
        private readonly IRouteRepository _repository;


        public AdminGetRouteHandler(IRouteRepository repository)
        {
            _repository = repository;
        }

        public async Task<CommandResponse<RouteDateResponse>> Handle(AdminGetRouteCommand request, CancellationToken cancellationToken)
        {
            var result = (await _repository.GetAsync(
                p => p.Id == request.Id,
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
            )).LastOrDefault();

            if (result == null)
            {
                return new NoAcess<RouteDateResponse>
                {
                    Message = "No active route was found. Please start a new route."
                };
            }


            var input = result.Items
                 .Where(x => x?.ServiceAppointmentLocation?.LocationCompany?.Lat != null
                    && x.ServiceAppointmentLocation.LocationCompany.Long != null)
                .ToList();

            double radiusMiles = 0.03; // ~158 meters
            double radiusMeters = radiusMiles * LocationCompany.MetersPerMile;



            var routes = input.Select(x =>
            {
                var srv = x.ServiceAppointmentLocation;
                var lc = x.ServiceAppointmentLocation.LocationCompany!;
                var lat = lc.Lat;
                var lng = lc.Long;

                return new RouteLocationResponse
                {
                    Id = srv.Id,
                    RouteId = result.Id,
                    Address = lc.Address,
                    PrimaryFirstName = lc.PrimaryFirstName,
                    PrimaryLastName = lc.PrimaryLastName,
                    PrimaryPhonNumber = lc.PrimaryPhonNumber,
                    LocationCompanyId = lc.Id,
                    Lat = lat,
                    Lng = lng,
                    CompanyName = lc.Company?.CompanyName,
                    Status = srv.Status,
                    ManifestNumber = x.ManifestNumber,
                    Services = new RouteServices
                    {
                        ProductName = x.ServiceAppointmentLocation!.ServiceAppointment!.Product?.Name,
                        ProductPriceName = x.ServiceAppointmentLocation!.ServiceAppointment!.ProductPrice?.Name,
                        IsEmegency = x.ServiceAppointmentLocation!.ServiceAppointment!.IsEmegency,
                        Capacity = x.ServiceAppointmentLocation!.ServiceAppointment!.Qty,
                        ServiceType = x.ServiceAppointmentLocation!.ServiceAppointment!.Product is { } p
                            ? ((ServiceType)p.ServiceType).GetDescription()
                            : null,
                        Status = x.ServiceAppointmentLocation!.Status,
                        FinishDate = x.ServiceAppointmentLocation!.FinishDate,
                        StartedAt = x.ServiceAppointmentLocation!.StartedAt,
                        ServiceAppointmentLocationId = x.ServiceAppointmentLocation!.Id,
                    }
                };
            }).ToList();

            var routeResponse = new RouteDateResponse
            {
                Id = result.Id,
                Name = result.Name,
                Date = result.Date,
                Routes = routes
            };

            //var http = new HttpClient();
            //var service = new GoogleDirectionsService("AIzaSyB4RuysVF06sulgiowRTkToZ-M6gk9uxNU");

            //var mainpointList = routeResponse.Routes;


            //var waypoints = mainpointList.Count > 2
            //? mainpointList.GetRange(1, mainpointList.Count - 2) :
            //new List<RouteLocationResponse>();

            //var cmd = new DirectionCommand
            //{
            //    Origin = new LatLngPoint()
            //    {
            //        Lat = request.Lat ?? mainpointList.First().Lat,
            //        Lng = request.Lng ?? mainpointList.First().Lng,
            //    },
            //    Destination = new LatLngPoint()
            //    {
            //        Lat = mainpointList.Last().Lat,
            //        Lng = mainpointList.Last().Lng,
            //    },
            //    Waypoints = waypoints.Select(p =>
            //    new LatLngPoint()
            //    {
            //        Lat = p.Lat,
            //        Lng = p.Lng,
            //    }).ToList()
            //};

            //DirectionsResult r = await service.GetDirectionsAsync(cmd);
            //routeResponse.DirectionsResult = r;

            return new Success<RouteDateResponse>()
            {
                Data = routeResponse,
            };

        }




    }
}


//public class DirectionCommand
//{
//    public LatLngPoint Origin { get; set; } = new();
//    public LatLngPoint Destination { get; set; } = new();
//    public List<LatLngPoint> Waypoints { get; set; } = new();
//}

//public class LatLngPoint
//{
//    public double Lat { get; set; }
//    public double Lng { get; set; }
//}