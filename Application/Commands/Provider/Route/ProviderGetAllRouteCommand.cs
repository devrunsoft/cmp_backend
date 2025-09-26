using System;
using CMPNatural.Core.Base;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;
using ScoutDirect.Core.Base;

namespace CMPNatural.Application
{
	public class ProviderGetAllRouteCommand : PagedQueryRequest, IRequest<CommandResponse<PagesQueryResponse<Route>>>
    {
        public long ProviderId { get; set; }
    }
}

