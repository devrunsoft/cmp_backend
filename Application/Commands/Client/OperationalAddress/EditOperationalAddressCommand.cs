using System;
using CMPNatural.Application.Model;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Commands.OperationalAddress
{
	public class EditOperationalAddressCommand : OperationalAddressInput, IRequest<CommandResponse<object>>
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
    }
}

