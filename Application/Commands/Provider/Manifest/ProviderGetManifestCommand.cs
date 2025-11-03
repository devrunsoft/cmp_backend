using System;
using CMPNatural.Application.Responses;
using CMPNatural.Core.Base;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Enums;
using MediatR;
using ScoutDirect.Application.Responses;
using ScoutDirect.Core.Base;

namespace CMPNatural.Application
{
    public class ProviderGetManifestCommand : IRequest<CommandResponse<ManifestResponse>>
    {
        public ProviderGetManifestCommand()
        {
        }
        public long Id { get; set; }
        public long ProviderId { get; set; }
    }
}

