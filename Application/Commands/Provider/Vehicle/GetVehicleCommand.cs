using System;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Collections.Generic;
using CMPNatural.Core.Entities;

namespace CMPNatural.Application
{

    public class GetVehicleCommand : IRequest<CommandResponse<Vehicle>>
    {
        public GetVehicleCommand()
        {
        }

        public long ProviderId { get; set; }
        public long Id { get; set; }
    }
}


