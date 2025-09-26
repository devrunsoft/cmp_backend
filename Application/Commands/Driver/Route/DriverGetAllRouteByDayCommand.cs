using System;
using CMPNatural.Application.Responses.Driver;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Collections.Generic;

namespace CMPNatural.Application.Commands.Driver.Route
{

    public class DriverGetAllRouteByDayCommand : IRequest<CommandResponse<List<RouteDateResponse>>>
    {
        public long DriverId { get; set; }
        public DateTime Date { get; set; }
    }
}

