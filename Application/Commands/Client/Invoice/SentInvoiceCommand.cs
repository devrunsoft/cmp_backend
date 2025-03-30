using System;
using CMPNatural.Core.Enums;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Commands.Invoice
{
	public class SentInvoiceCommand : IRequest<CommandResponse<object>>
    {
        public long InvoiceId { get; set; }
        //public InvoiceStatus Status { get; set; }
        public long CompanyId { get; set; }
    }
}

