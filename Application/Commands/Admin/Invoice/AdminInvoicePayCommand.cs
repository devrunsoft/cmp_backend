using System;
using CMPNatural.Application.Responses.Report;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
    public class AdminInvoicePayCommand : IRequest<CommandResponse<InvoiceResponse>>
    {
        public AdminInvoicePayCommand()
        {
        }
        public long InvoiceId { get; set; }
    }
}

