using System;
using CMPNatural.Application.Model;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Commands.Billing
{
    public class GetBilingInformationCommand : IRequest<CommandResponse<BillingInformation>>
    {
        public long CompanyId { get; set; }
    }
}

