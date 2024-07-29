using Bazaro.Application.Commands;
using Bazaro.Application.Responses.Base;
using Bazaro.Core.Repositories;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Bazaro.Application.Handlers.CommandHandlers
{
    public class SeenAllNotificationHandler : IRequestHandler<SeenAllNotificationCommand, CommandResponse>
    {
        private readonly INotificationRepository _notificationRepository;

        public SeenAllNotificationHandler(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<CommandResponse> Handle(SeenAllNotificationCommand request, CancellationToken cancellationToken)
        {
            var notifications = await _notificationRepository.GetAllUnSeenNotificationsListAsync(request.ReciverPersonId);
            foreach (var notification in notifications)
            {
                if (notification.ReciveTime == null)
                {
                    notification.ReciveTime = DateTime.Now;
                }

                notification.SeenTime = DateTime.Now;
                await _notificationRepository.UpdateAsync(notification);
            } 
              
            return new Success() { };
        }
    }

}
