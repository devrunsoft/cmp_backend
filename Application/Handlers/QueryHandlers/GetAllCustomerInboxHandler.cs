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
    public class GetAllCustomerInboxHandler : IRequestHandler<GetAllCustomerInboxQuery, List<InboxResponse>>
    {
        private readonly IInboxRepository _inboxRepo;

        public GetAllCustomerInboxHandler(IInboxRepository inboxRepository)
        {
            _inboxRepo = inboxRepository;
        }

        public async Task<List<InboxResponse>> Handle(GetAllCustomerInboxQuery request, CancellationToken cancellationToken)
        {
            var Result = await _inboxRepo.GetCustomerInbox(request, (int)request.CustomerId, request.Id);

            return Result.Select(p => InboxCustomerMapper.Mapper.Map<InboxResponse>(p)).ToList();
        }
    }
}
