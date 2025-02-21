using System;
using CMPNatural.Application.Model;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
	public class EditVehicleCommand : VehicleInput, IRequest<CommandResponse<Vehicle>>
    {
		public EditVehicleCommand()
		{
		}
		public long Id { get; set; }
        public long ProviderId { get; set; }
        public string BaseVirtualPath { get; set; }
    }
}

