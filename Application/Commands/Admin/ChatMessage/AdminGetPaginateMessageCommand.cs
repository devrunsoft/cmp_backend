using System;
using CMPNatural.Core.Base;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;
using ScoutDirect.Core.Base;

namespace CMPNatural.Application
{
    public class AdminGetPaginateMessageCommand : PagedQueryRequest, IRequest<CommandResponse<PagesQueryResponse<ChatMessage>>>
    {
        public AdminGetPaginateMessageCommand()
        {
        }
        public long CompanyId { get; set; }
    }
}

