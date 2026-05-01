using System;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
    public class ProviderLoginGoogleCommand : IRequest<CommandResponse<Provider>>
    {
        public string Credential { get; set; }
    }
}

