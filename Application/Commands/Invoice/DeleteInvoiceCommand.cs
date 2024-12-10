using System;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Commands.Invoice
{
	public class DeleteInvoiceCommand : IRequest<CommandResponse<object>>
    {
        public long InvoiceId { get; set; }
        public long CompanyId { get; set; }
    }
}

