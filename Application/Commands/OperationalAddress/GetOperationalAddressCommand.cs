using System;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Commands.OperationalAddress
{
	public class GetOperationalAddressCommand : IRequest<CommandResponse<object>>
    {
		public GetOperationalAddressCommand()
		{
		}

		public long CompanyId { get; set; }
	}
}

