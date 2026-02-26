using MediatR;
using ScoutDirect.Application.Responses;
using CMPNatural.Core.Entities;

namespace CMPNatural.Application
{
    public class SignProviderContractCommand : IRequest<CommandResponse<ProviderContract>>
    {
        public SignProviderContractCommand()
        {
        }
        public long ProviderId { get; set; }
        public long ProviderContractId { get; set; }
        public string Sign { get; set; }
    }
}

