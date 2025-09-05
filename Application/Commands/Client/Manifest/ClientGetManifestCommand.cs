using System;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
    public class ClientGetManifestCommand : IRequest<CommandResponse<Manifest>>
    {
        public ClientGetManifestCommand()
        {
        }
        public long Id { get; set; }
    }
}

