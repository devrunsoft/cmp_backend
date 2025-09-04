using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
    public class DriverUpComingManifestCommand : IRequest<CommandResponse<Manifest>>
    {
        public long DriverId { get; set; }
    }
}

