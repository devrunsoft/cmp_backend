using System;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Commands.Driver
{
    public class DriverGoogleLoginCommand : IRequest<CommandResponse<DriverResponse>>
    {
        public string Credential { get; set; }
    }
}

