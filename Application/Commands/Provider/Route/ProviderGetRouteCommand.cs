using System;
using CMPNatural.Core.Base;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Commands.Provider
{
	public class ProviderGetRouteCommand : IRequest<CommandResponse<Route>>
    {
        public long ProviderId { get; set; }
        public long RouteId { get; set; }
    }
}

