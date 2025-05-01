using System;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
    public class AdminActivateInvoiceCommand : IRequest<CommandResponse<InvoiceResponse>>
    {
        public long InvoiceId { get; set; }
    }
}

