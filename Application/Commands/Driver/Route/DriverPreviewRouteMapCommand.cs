using System;
using CMPNatural.Application.Responses.Driver;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Commands.Driver.Route
{
	public class DriverPreviewRouteMapCommand : IRequest<CommandResponse<RouteDateResponse>>
    {
		public long RouteId { get; set; }
		public long DriverId { get; set; }
        public double? Lat { get; set; }
        public double? Lng { get; set; }
    }
}

