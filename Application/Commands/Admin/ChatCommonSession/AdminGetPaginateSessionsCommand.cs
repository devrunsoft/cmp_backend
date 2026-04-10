using System;
using System.Collections.Generic;
using CMPNatural.Core.Base;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;
using ScoutDirect.Core.Base;

namespace CMPNatural.Application
{
    public class AdminGetPaginateCommonMessageSessionsCommand : PagedQueryRequest, IRequest<CommandResponse<List<ChatSession>>>
    {
        public AdminGetPaginateCommonMessageSessionsCommand()
        {
        }
        public long AdminId { get; set; }
        public long CommonSessionId { get; set; }
    }
}

