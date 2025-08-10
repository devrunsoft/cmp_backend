using System;
using CMPNatural.Core.Base;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;
using ScoutDirect.Core.Base;

namespace CMPNatural.Application
{
    public class AdminGetPaginateMessageManualNoteCommand : PagedQueryRequest, IRequest<CommandResponse<PagesQueryResponse<ChatMessageManualNote>>>
    {
        public AdminGetPaginateMessageManualNoteCommand()
        {
        }
        public long OperationalAddressId { get; set; }
    }
}

