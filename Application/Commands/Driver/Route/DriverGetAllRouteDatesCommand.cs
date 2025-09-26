using System;
using CMPNatural.Application.Responses.Driver;
using System.Collections.Generic;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
    public class DriverGetAllRouteDatesCommand : IRequest<CommandResponse<List<RouteDateResponse>>>
    {
        public long DriverId { get; set; }
        public DateTime Date { get; set; }
    }
}

