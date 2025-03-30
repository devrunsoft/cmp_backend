using System;
using MediatR;
using ScoutDirect.Application.Responses;
using CMPNatural.Core.Entities;

namespace CMPNatural.Application
{
	public class GetOperationalAddressCommand : IRequest<CommandResponse<OperationalAddress>>
    {

		public long CompanyId { get; set; }
		//public long Id { get; set; }
	}
}

