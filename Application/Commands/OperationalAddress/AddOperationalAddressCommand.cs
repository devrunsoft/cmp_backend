using System;
using CMPNatural.Application.Model;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Commands.OperationalAddress
{
	public class AddOperationalAddressCommand : OperationalAddressInput, IRequest<CommandResponse<object>>
    {
		public long CompanyId { get; set; }
	}
}

