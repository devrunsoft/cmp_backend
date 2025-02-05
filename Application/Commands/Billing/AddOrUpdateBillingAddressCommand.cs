using System;
using CMPNatural.Application.Model;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Commands.Billing
{
	public class AddOrUpdateBillingAddressCommand : IRequest<CommandResponse<BillingInformation>>
    {
        public long CompanyId { get; set; }

        public string Address { get; set; }

        public string? City { get; set; } = "";

        public string? ZIPCode { get; set; } = "";

        public string? State { get; set; } = "";
    }
}

