using System;
using CMPNatural.Application.Model;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
    public class ProviderUpdateInvoiceCommand : ProviderInvoiceInput, IRequest<CommandResponse<Invoice>>
    {
        public ProviderUpdateInvoiceCommand()
        {
        }
        public long InvoiceId { get; set; }
        public long ProviderId { get; set; }
    }
}

