using System;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
    public class DriverStartRouteManifestCommand : IRequest<CommandResponse<Manifest>>
    {
        public long DriverId { get; set; }
        public long ManifestId { get; set; }
    }
}

