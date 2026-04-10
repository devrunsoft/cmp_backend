using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
    public class DeleteVehicleOfProviderCommand : IRequest<CommandResponse<Vehicle>>
    {
        public long VehicleId { get; set; }
        public long ProviderId { get; set; }
    }
}
