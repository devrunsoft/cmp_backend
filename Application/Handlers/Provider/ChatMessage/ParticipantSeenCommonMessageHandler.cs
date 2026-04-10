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
using CMPNatural.Core.Repositories.ChatCommon;

namespace CMPNatural.Application
{
    public class ParticipantSeenCommonMessageHandler : IRequestHandler<ParticipantSeenCommonMessageCommand, CommandResponse<ChatCommonMessage>>
    {
        private readonly IChatCommonMessageRepository _repository;
        private readonly IChatService _chatService;

        public ParticipantSeenCommonMessageHandler(IChatCommonMessageRepository _repository , IChatService _chatService)
        {
            this._repository = _repository;
            this._chatService = _chatService;
        }

        public async Task<CommandResponse<ChatCommonMessage>> Handle(ParticipantSeenCommonMessageCommand request, CancellationToken cancellationToken)
        {
            var result = (await _repository.GetAsync(x => x.Id == request.ChatMessageId && x.SenderId != request.ParticipantId)).FirstOrDefault();
            if (result == null)
            {
                return new NoAcess<ChatCommonMessage>();
            }
            result.IsSeen = true;
            await _repository.UpdateAsync(result);

            await _chatService.SendCommonToAllAdmins(result, ChatEnum.seen);
            return new Success<ChatCommonMessage>() { Data = result };
        }
    }
}

