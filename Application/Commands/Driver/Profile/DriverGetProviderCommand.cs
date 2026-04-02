using System;
using System.Collections.Generic;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
	public class DriverGetProviderCommand : IRequest<CommandResponse<List<Provider>>>
    {
        public long DriverId { get; set; }
    }
}

