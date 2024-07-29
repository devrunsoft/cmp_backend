using Bazaro.Application.Commands;
using Bazaro.Application.Responses.Base;
using Bazaro.Core.Repositories;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Bazaro.Application.Handlers.CommandHandlers
{
    public class SeenAllChatHandler : IRequestHandler<SeenAllChatCommand, CommandResponse>
    {
        private readonly IChatRepository _chatRepository;

        public SeenAllChatHandler(IChatRepository chatRepository)
        {
            _chatRepository = chatRepository;
        }

        public async Task<CommandResponse> Handle(SeenAllChatCommand request, CancellationToken cancellationToken)
        {
            var notifications = await _chatRepository.GetAllUnSeenChatsListAsync(request.InboxId);
            foreach (var notification in notifications)
            {
                notification.Seen = true;
                await _chatRepository.UpdateAsync(notification);
            }

            return new Success() { Data = notifications };
        }
    }

}
