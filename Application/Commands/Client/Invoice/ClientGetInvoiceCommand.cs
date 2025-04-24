using System;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
    public class ClientGetInvoiceCommand : IRequest<CommandResponse<InvoiceResponse>>
    {
        public ClientGetInvoiceCommand()
        {
        }
        public long InvoiceId { get; set; }
    }
}

