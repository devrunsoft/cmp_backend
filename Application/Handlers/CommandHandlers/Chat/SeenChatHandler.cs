using Bazaro.Application.Commands;
using Bazaro.Application.Responses.Base;
using Bazaro.Core.Repositories;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Bazaro.Application.Handlers.CommandHandlers
{
    public class SeenChatHandler : IRequestHandler<SeenChatCommand, CommandResponse>
    {
        private readonly IChatRepository _chatRepository;

        public SeenChatHandler(IChatRepository chatRepository)
        {
            _chatRepository = chatRepository;
        }

        public async Task<CommandResponse> Handle(SeenChatCommand request, CancellationToken cancellationToken)
        {
            var chat = await _chatRepository.GetByIdAsync(request.Id);
            if (chat == null)
                return new ResponseNotFound();

            chat.Seen = true;

            await _chatRepository.UpdateAsync(chat);

            return new Success() { Data = chat };
        }
    }

}
