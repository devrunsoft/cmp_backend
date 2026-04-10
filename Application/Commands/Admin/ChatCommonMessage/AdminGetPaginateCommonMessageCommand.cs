using System;
using CMPNatural.Core.Base;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;
using ScoutDirect.Core.Base;

namespace CMPNatural.Application
{
    public class AdminGetPaginateCommonMessageCommand : PagedQueryRequest, IRequest<CommandResponse<PagesQueryResponse<ChatCommonMessage>>>
    {
        public long ChatCommonSessionId { get; set; }
    }
}

