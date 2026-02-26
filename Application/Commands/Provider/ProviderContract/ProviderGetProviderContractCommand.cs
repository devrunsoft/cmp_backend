using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
    public class ProviderGetProviderContractCommand : IRequest<CommandResponse<ProviderContract>>
    {
        public ProviderGetProviderContractCommand()
        {
        }
        public long Id { get; set; }

        public long? ProviderId { get; set; }
    }
}

