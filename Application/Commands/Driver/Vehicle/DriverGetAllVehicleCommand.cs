using System.Collections.Generic;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
	public class DriverGetAllVehicleCommand : IRequest<CommandResponse<List<Vehicle>>>
    {
		public DriverGetAllVehicleCommand()
		{
		}
		public long DriverId { get; set; }
	}
}

