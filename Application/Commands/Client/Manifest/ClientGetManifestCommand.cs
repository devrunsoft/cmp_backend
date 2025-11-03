using System;
using CMPNatural.Application.Responses;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
    public class ClientGetManifestCommand : IRequest<CommandResponse<ManifestResponse>>
    {
        public ClientGetManifestCommand()
        {
        }
        public long Id { get; set; }
    }
}

