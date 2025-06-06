using System;
using CMPNatural.Application.Model;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Enums;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
    public class ProviderSubmitInvoiceCommand : IRequest<CommandResponse<Invoice>>
    {
        public ProviderSubmitInvoiceCommand()
        {
        }
        public long InvoiceId { get; set; }
        public long ProviderId { get; set; }
        public string Comment { get; set; }
        public OilQualityEnum? OilQuality { get; set; }
    }
}

