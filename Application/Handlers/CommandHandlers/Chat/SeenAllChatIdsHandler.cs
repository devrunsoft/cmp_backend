using Bazaro.Application.Commands;
using Bazaro.Application.Responses.Base;
using Bazaro.Core.Repositories;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Bazaro.Application.Handlers.CommandHandlers
{
    public class SeenAllChatIdsHandler : IRequestHandler<SeenAllChatIdsCommand, CommandResponse>
    {
        private readonly IChatRepository _chatRepository;

        public SeenAllChatIdsHandler(IChatRepository chatRepository)
        {
            _chatRepository = chatRepository;
        }

        public async Task<CommandResponse> Handle(SeenAllChatIdsCommand request, CancellationToken cancellationToken)
        {
            var chats = await _chatRepository.GetAllChatIdsListAsync(request.Ids);
            foreach (var chat in chats)
            {
                chat.Seen = true;
                await _chatRepository.UpdateAsync(chat);
            }

            return new Success() { Data = chats };
        }
    }

}
