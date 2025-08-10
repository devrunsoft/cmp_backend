using System;
using System.Collections.Generic;
using CMPNatural.Core.Base;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;
using ScoutDirect.Core.Base;

namespace CMPNatural.Application
{
    public class AdminGetPaginateClientSessionsCommand : PagedQueryRequest, IRequest<CommandResponse<List<ChatClientSession>>>
    {
        public AdminGetPaginateClientSessionsCommand()
        {
        }
        public long AdminId { get; set; }
    }
}

