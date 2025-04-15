using System.Collections.Generic;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
    public class ProviderGetAllActiveManifestCommand : IRequest<CommandResponse<List<Manifest>>>
    {
        public ProviderGetAllActiveManifestCommand()
        {
        }
        public long ProviderId { get; set; }
    }
}

