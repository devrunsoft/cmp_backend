using System;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Enums;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
    public class AdminSentInvoiceCommand : IRequest<CommandResponse<Invoice>>
    {
        public long InvoiceId { get; set; }
        public InvoiceStatus Status { get; set; }
    }
}

