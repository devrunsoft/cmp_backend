using System;
using System.Collections.Generic;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Commands.Provider.Driver
{
	public class CheckDriverByEmailCommand: IRequest<CommandResponse<List<DriverResponse>>>
    {
		public string Email { get; set; }
	}
}

