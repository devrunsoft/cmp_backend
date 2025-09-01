using System;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Collections.Generic;

namespace CMPNatural.Application
{

    public class GetDriverCommand : IRequest<CommandResponse<DriverResponse>>
    {
        public GetDriverCommand()
        {
        }

        public long ProviderId { get; set; }
        public long Id { get; set; }
    }
}

