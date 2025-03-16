using System;
using CMPNatural.Application.Model;
using CMPNatural.Core.Base;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;
using ScoutDirect.Core.Base;

namespace CMPNatural.Application
{
    public class AdminPostProviderCommand : ProviderInput , IRequest<CommandResponse<Provider>>
    {
        public string BaseVirtualPath { get; set; }
    }
}

