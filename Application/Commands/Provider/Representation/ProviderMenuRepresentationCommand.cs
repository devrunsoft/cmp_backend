using CMPNatural.Application.Responses;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Commands.Provider.Representation
{
	public class ProviderMenuRepresentationCommand : IRequest<CommandResponse<ProviderMenuRepresentationResponse>>
    {
        public long ProviderId { get; set; }
        public long? DriverId { get; set; }
        public bool IsDriver { get; set; }
    }
}       

