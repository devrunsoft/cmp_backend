using System;
using CMPNatural.Core.Enums;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Commands.Admin.Invoice
{
    public class AdminSentInvoiceCommand : IRequest<CommandResponse<object>>
    {
        public long InvoiceId { get; set; }
        public InvoiceStatus Status { get; set; }
    }
}

