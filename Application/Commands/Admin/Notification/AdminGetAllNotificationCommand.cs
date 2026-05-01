using System;
using CMPNatural.Core.Base;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;
using ScoutDirect.Core.Base;

namespace CMPNatural.Application
{
    public class AdminGetAllNotificationCommand : PagedQueryRequest, IRequest<CommandResponse<PagesQueryResponse<Notification>>>
    {
        public AdminGetAllNotificationCommand()
        {
        }
    }
}

