using System;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Collections.Generic;

namespace CMPNatural.Application
{
    public class AdminSetInvoiceProviderCommand : IRequest<CommandResponse<Manifest>>
    {
        public AdminSetInvoiceProviderCommand()
        {
        }
        public long ManifestId { get; set; }
        public long ProviderId { get; set; }
    }
}

