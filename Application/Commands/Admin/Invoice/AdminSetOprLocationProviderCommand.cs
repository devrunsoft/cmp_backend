using System;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Collections.Generic;

namespace CMPNatural.Application
{

    public class AdminSetOprLocationProviderCommand : IRequest<CommandResponse<Invoice>>
    {
        public AdminSetOprLocationProviderCommand()
        {
        }
        public long ServiceId { get; set; }
        public string InvoiceId { get; set; }
        public long ProviderId { get; set; }
    }
}

