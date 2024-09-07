using System;
using CMPNatural.Application.Model;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
	public class AddOperationalAddressCommand : OperationalAddressInput, IRequest<CommandResponse<object>>
    {
		public long CompanyId { get; set; }
	}
}

