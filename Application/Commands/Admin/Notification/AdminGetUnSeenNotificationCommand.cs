using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Commands.Admin.Notification
{
    public class AdminGetUnSeenNotificationCommand :  IRequest<CommandResponse<int>>
    {
        public AdminGetUnSeenNotificationCommand()
        {
        }
    }
}

