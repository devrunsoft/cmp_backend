using System;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Commands.Provider.Driver
{
	public class DeleteDriverOfProviderCommand: IRequest<CommandResponse<DriverResponse>>
    {
		public long DriverId { get; set; }
		public long ProviderId { get; set; }
	}
}

