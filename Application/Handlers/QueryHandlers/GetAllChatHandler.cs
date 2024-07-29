using Bazaro.Application.Mapper;
using Bazaro.Application.Queries;
using Bazaro.Application.Responses;
using Bazaro.Core.Repositories;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Bazaro.Application.Handlers.QueryHandlers
{
    public class GetAllChatHandler : IRequestHandler<GetAllChatQuery, List<ChatResponse>>
    {
        private readonly IChatRepository _chatRepository;

        public GetAllChatHandler(IChatRepository chatRepository)
        {
            _chatRepository = chatRepository;
        }

        public async Task<List<ChatResponse>> Handle(GetAllChatQuery request, CancellationToken cancellationToken)
        {
            var Result = await _chatRepository.GetInboxChatPagedAsync(request.InboxId, request);
            return Result.Select(p => ChatMapper.Mapper.Map<ChatResponse>(p)).ToList();
        }
    }
}
