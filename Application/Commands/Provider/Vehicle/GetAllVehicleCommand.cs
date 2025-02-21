using System;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Collections.Generic;

namespace CMPNatural.Application
{
	public class GetAllVehicleCommand : IRequest<CommandResponse<List<Vehicle>>>
    {
		public GetAllVehicleCommand()
		{
		}

		public long ProviderId { get; set; }
	}
}

