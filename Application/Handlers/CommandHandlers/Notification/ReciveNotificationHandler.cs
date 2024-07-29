using Bazaro.Application.Commands;
using Bazaro.Application.Responses.Base;
using Bazaro.Core.Repositories;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Bazaro.Application.Handlers.CommandHandlers
{
    public class ReciveNotificationHandler : IRequestHandler<ReciveNotificationCommand, CommandResponse>
    {
        private readonly INotificationRepository _notificationRepository;

        public ReciveNotificationHandler(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<CommandResponse> Handle(ReciveNotificationCommand request, CancellationToken cancellationToken)
        {
            var notification = await _notificationRepository.GetByIdAsync(request.Id);
            if (notification == null)
                return new ResponseNotFound();

            notification.ReciveTime = DateTime.Now; 

            await _notificationRepository.UpdateAsync(notification);

            return new Success() { };
        }
    }

}
