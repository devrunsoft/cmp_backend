using MediatR;
using ScoutDirect.Application.Responses;
using CMPNatural.Core.Entities;

namespace CMPNatural.Application
{
    public class AdminSignProviderContractCommand : IRequest<CommandResponse<ProviderContract>>
    {
        public AdminSignProviderContractCommand()
        {
        }
        public long ProviderId { get; set; }
        public long ProviderContractId { get; set; }
    }
}

