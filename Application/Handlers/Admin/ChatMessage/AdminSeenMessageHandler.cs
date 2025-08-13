using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Repositories.Chat;
using System.Linq;
using ScoutDirect.Core.Entities.Base;
using CMPNatural.Application.Hub;

namespace CMPNatural.Application
{
    public class AdminSeenMessageHandler : IRequestHandler<AdminSeenMessageCommand, CommandResponse<ChatMessage>>
    {
        private readonly IChatMessageRepository _repository;
        private readonly IChatService _chatService;
        public AdminSeenMessageHandler(IChatMessageRepository _repository , IChatService _chatService)
        {
            this._repository = _repository;
            this._chatService = _chatService;
        }

        public async Task<CommandResponse<ChatMessage>> Handle(AdminSeenMessageCommand request, CancellationToken cancellationToken)
        {
            var result = (await _repository.GetAsync(x=>x.Id == request.ChatMessageId && x.SenderId != request.AdminId)).FirstOrDefault();
            if (result == null)
            {
                return new NoAcess<ChatMessage>();
            }
            result.IsSeen = true;
            await _repository.UpdateAsync(result);

            await _chatService.SendMessageToClient(result.ClientId, result , ChatEnum.seen);
            return new Success<ChatMessage>() { Data = result };
        }
    }
}

