using System.Collections.Generic;
using CMPNatural.Application.Responses.ChatCommon;
using MediatR;
using ScoutDirect.Application.Responses;
using ScoutDirect.Core.Base;

namespace CMPNatural.Application
{
    public class AdminGetPaginateCommonSessionsCommand : PagedQueryRequest, IRequest<CommandResponse<List<ChatCommonSessionEntity>>>
    {
        public long AdminId { get; set; }
    }
}
