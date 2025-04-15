using System.Collections.Generic;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
    public class ProviderGetAllCompleteManifestCommand : IRequest<CommandResponse<List<Manifest>>>
    {
        public ProviderGetAllCompleteManifestCommand()
        {
        }
        public long ProviderId { get; set; }
    }
}

