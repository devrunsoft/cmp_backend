using System;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Commands.Admin.Invoice
{
    public class AdminSentInvoiceCommand : IRequest<CommandResponse<object>>
    {
        public long InvoiceId { get; set; }
        public string Status { get; set; }
    }
}

