using System;
using CMPNatural.Core.Base;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Enums;
using MediatR;
using ScoutDirect.Application.Responses;
using ScoutDirect.Core.Base;

namespace CMPNatural.Application
{
    public class AdminAddNotificationCommand : IRequest<CommandResponse<Notification>>
    {
        public string Title { get; set; } = string.Empty;

        public string Body { get; set; } = string.Empty;

        public NotificationType type { get; set; }

        public object? Payload { get; set; } = string.Empty;
    }
}

