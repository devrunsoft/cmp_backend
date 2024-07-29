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
    public class GetAllInboxHandler : IRequestHandler<GetAllInboxQuery, List<InboxResponse>>
    {
        private readonly IInboxRepository _inboxRepo;

        public GetAllInboxHandler(IInboxRepository inboxRepository)
        {
            _inboxRepo = inboxRepository;
        }

        public async Task<List<InboxResponse>> Handle(GetAllInboxQuery request, CancellationToken cancellationToken)
        {
            var Result = await _inboxRepo.GetInbox(request, (int)request.ShopId, request.Id);

            return Result.Select(p => InboxMapper.Mapper.Map<InboxResponse>(p)).ToList();
        }
    }
}
