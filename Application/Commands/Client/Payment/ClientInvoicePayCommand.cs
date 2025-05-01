using System;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
    public class ClientInvoicePayCommand : IRequest<CommandResponse<string>>
    {
        public ClientInvoicePayCommand()
        {
        }
        public long InvoiceId { get; set; }
    }
}

