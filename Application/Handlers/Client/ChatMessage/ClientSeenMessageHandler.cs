using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Repositories.Chat;
using System.Linq;
using CMPNatural.Application.Hub;
using ScoutDirect.Core.Entities.Base;
using ScoutDirect.Core.Repositories.Base;

namespace CMPNatural.Application
{
    public class ClientSeenMessageHandler : IRequestHandler<ClientSeenMessageCommand, CommandResponse<ChatMessage>>
    {
        private readonly IChatMessageRepository _repository;
        private readonly IChatService _chatService;

        public ClientSeenMessageHandler(IChatMessageRepository _repository , IChatService _chatService)
        {
            this._repository = _repository;
            this._chatService = _chatService;
        }

        public async Task<CommandResponse<ChatMessage>> Handle(ClientSeenMessageCommand request, CancellationToken cancellationToken)
        {
            var result = (await _repository.GetAsync(x => x.Id == request.ChatMessageId && x.SenderId != request.ClientId)).FirstOrDefault();
            if (result == null)
            {
                return new NoAcess<ChatMessage>();
            }
            result.IsSeen = true;
            await _repository.UpdateAsync(result);

            await _chatService.SendToAllAdmins(result, ChatEnum.seen);
            return new Success<ChatMessage>() { Data = result };
        }
    }
}

