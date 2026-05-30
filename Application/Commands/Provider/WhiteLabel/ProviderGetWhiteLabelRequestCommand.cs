using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Commands.Provider.WhiteLabel
{
    public class ProviderGetWhiteLabelRequestCommand : IRequest<CommandResponse<WhiteLabelRequest>>
    {
        public long ProviderId { get; set; }
    }
}
