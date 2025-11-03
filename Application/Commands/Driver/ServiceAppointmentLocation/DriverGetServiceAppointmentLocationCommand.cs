using System.Collections.Generic;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
	public class DriverGetServiceAppointmentLocationCommand : IRequest<CommandResponse<List<ServiceAppointmentLocation>>>
    {
        public long DriverId { get; set; }
        public long RouteId { get; set; }

    }
}

