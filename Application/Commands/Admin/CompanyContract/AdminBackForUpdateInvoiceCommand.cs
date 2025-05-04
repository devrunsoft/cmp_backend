using System;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Enums;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
	public class AdminBackForUpdateInvoiceCommand : IRequest<CommandResponse<InvoiceResponse>>
    {
        public long CompanyContractId { get; set; }
    }
}

