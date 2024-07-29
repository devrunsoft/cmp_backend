using Bazaro.Application.Commands;
using Bazaro.Application.Responses.Base;
using Bazaro.Core.Repositories;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Bazaro.Application.Handlers.CommandHandlers
{
    public class SeenNotificationHandler : IRequestHandler<SeenNotificationCommand, CommandResponse>
    {
        private readonly INotificationRepository _notificationRepository;

        public SeenNotificationHandler(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<CommandResponse> Handle(SeenNotificationCommand request, CancellationToken cancellationToken)
        {
            var notification = await _notificationRepository.GetByIdAsync(request.Id);
            if (notification == null)
                return new ResponseNotFound();

            notification.SeenTime = DateTime.Now;

            await _notificationRepository.UpdateAsync(notification);

            return new Success() { };
        }
    }

}
