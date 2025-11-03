using System;
using CMPNatural.Application.Responses.Driver;
using CMPNatural.Core.Base;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;
using ScoutDirect.Core.Base;

namespace CMPNatural.Application
{

    public class AdminGetRouteCommand : PagedQueryRequest, IRequest<CommandResponse<RouteDateResponse>>
    {
        public long AdminId { get; set; }
        public long Id { get; set; }
    }
}

