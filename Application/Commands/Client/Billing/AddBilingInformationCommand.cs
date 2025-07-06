using System;
using System.Collections.Generic;
using CMPNatural.Application.Model;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Commands
{
	public class AddBilingInformationCommand : IRequest<CommandResponse<BillingInformation>>
    {
        public string CorporateAddress { get; set; }
        public List<BilingInformationInput> BilingInformationInputs { get; set; }
        public long CompanyId { get; set; }
    }
}

