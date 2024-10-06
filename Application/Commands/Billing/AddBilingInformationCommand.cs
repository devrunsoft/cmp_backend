using System;
using CMPNatural.Application.Model;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Commands
{
	public class AddBilingInformationCommand : BilingInformationInput, IRequest<CommandResponse<BillingInformation>>
    {
        public long CompanyId { get; set; }
    }
}

