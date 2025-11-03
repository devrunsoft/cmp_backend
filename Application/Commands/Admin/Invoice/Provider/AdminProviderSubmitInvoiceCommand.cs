using System;
using CMPNatural.Application.Model;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
    public class AdminProviderSubmitInvoiceCommand : IRequest<CommandResponse<InvoiceResponse>>
    {
        public AdminProviderSubmitInvoiceCommand()
        {
        }
        public long InvoiceId { get; set; }
        public string Comment { get; set; }
    }
}