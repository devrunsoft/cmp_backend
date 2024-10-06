using System;
using MediatR;
using ScoutDirect.Application.Responses;
using CMPNatural.Core.Entities;

namespace CMPNatural.Application
{
	public class GetOperationalAddressCommand : IRequest<CommandResponse<OperationalAddress>>
    {
		public GetOperationalAddressCommand()
		{
		}

		public long CompanyId { get; set; }
	}
}

