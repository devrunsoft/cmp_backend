using System;
using CMPNatural.Application.Model;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Commands.Driver.Profile
{
    public class DriverUpdateProfileCommand : DriverInput, IRequest<CommandResponse<DriverResponse>>
    {
        public long DriverId { get; set; }
        public string BaseVirtualPath { get; set; }
    }
}

