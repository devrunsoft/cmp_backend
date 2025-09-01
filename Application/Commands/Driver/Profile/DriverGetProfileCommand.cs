using System;
using CMPNatural.Application.Model;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
	public class DriverGetProfileCommand : IRequest<CommandResponse<DriverResponse>>
    {
		public long DriverId { get; set; }
	}
}

