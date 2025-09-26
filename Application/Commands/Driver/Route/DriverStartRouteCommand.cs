using System;
using CMPNatural.Application.Responses.Driver;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Commands.Driver.Route
{
    public class DriverStartRouteCommand : IRequest<CommandResponse<RouteDateResponse>>
    {
        public DriverStartRouteCommand()
        {
        }
        public long DriverId { get; set; }
        public long RouteId { get; set; }
        public long VehicleId { get; set; }
    }
}

