using System;
using CMPNatural.Core.Base;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
    public class AdminChangeAssignManifestCommand : IRequest<CommandResponse<Manifest>>
    {
        public AdminChangeAssignManifestCommand()
        {
        }

        public long ProviderId { get; set; }
        public DateTime ServiceDateTime { get; set; }
        public long Id { get; set; }
    }
}

