using System;
using CMPNatural.Application.Model;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
	public class AddVehicleCommand : VehicleInput, IRequest<CommandResponse<Vehicle>>
    {
		public AddVehicleCommand()
		{
		}
        public long ProviderId { get; set; }
        public string BaseVirtualPath { get; set; }
    }
}

