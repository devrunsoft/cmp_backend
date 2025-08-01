using System;
using System.Collections.Generic;
using CMPNatural.Core.Base;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;
using ScoutDirect.Core.Base;

namespace CMPNatural.Application
{
    public class AdminGetPaginateSessionsCommand : PagedQueryRequest, IRequest<CommandResponse<List<ChatSession>>>
    {
        public AdminGetPaginateSessionsCommand()
        {
        }
        public long AdminId { get; set; }
    }
}

