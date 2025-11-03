using System;
using System.Collections.Generic;
using CMPNatural.Application.Responses.Driver;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Commands.Driver.Route
{
    public class DriverStartInProccessRouteCommand : IRequest<CommandResponse<RouteLocationResponse>>
    {
        public DriverStartInProccessRouteCommand()
        {
        }
        public long RouteServiceAppointmentLocationId { get; set; }
        public long RouteId { get; set; }
        public long DriverId { get; set; }
    }
}

