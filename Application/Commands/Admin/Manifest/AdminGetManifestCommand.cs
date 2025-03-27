using System;
using CMPNatural.Core.Base;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Enums;
using MediatR;
using ScoutDirect.Application.Responses;
using ScoutDirect.Core.Base;

namespace CMPNatural.Application
{
    public class AdminGetManifestCommand : IRequest<CommandResponse<Manifest>>
    {
        public AdminGetManifestCommand()
        {
        }
        public long Id { get; set; }
    }
}

