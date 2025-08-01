using System;
using CMPNatural.Core.Base;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;
using ScoutDirect.Core.Base;

namespace CMPNatural.Application
{
    public class ClientGetPaginateMessageCommand : PagedQueryRequest, IRequest<CommandResponse<PagesQueryResponse<ChatMessage>>>
    {
        public ClientGetPaginateMessageCommand()
		{
		}
        public long CompanyId { get; set; }
    }
}

