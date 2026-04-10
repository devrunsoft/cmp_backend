using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using CMPNatural.Application.Hub;
using CMPNatural.Core.Repositories.ChatCommon;
using CMPNatural.Core.Entities;

namespace CMPNatural.Application
{
    public class AdminSeenCommonMessageHandler : IRequestHandler<AdminSeenCommonMessageCommand, CommandResponse<ChatCommonMessage>>
    {
        private readonly IChatCommonMessageRepository _repository;
        private readonly IChatService _chatService;
        public AdminSeenCommonMessageHandler(IChatCommonMessageRepository _repository , IChatService _chatService)
        {
            this._repository = _repository;
            this._chatService = _chatService;
        }

        public async Task<CommandResponse<ChatCommonMessage>> Handle(AdminSeenCommonMessageCommand request, CancellationToken cancellationToken)
        {
            var result = (await _repository.GetAsync(x=>x.Id == request.ChatCommonMessageId && x.SenderId != request.AdminId)).FirstOrDefault();
            if (result == null)
            {
                return new NoAcess<ChatCommonMessage>();
            }
            result.IsSeen = true;
            await _repository.UpdateAsync(result);

            await _chatService.SendCommonMessageToPerson(result.PersonId.ToString(), result , ChatEnum.seen);
            return new Success<ChatCommonMessage>() { Data = result };
        }
    }
}

