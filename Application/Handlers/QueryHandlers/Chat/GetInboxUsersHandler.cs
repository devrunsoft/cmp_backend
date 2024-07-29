using Bazaro.Application.Mapper;
using Bazaro.Application.Queries;
using Bazaro.Application.Responses;
using Bazaro.Core.Models;
using Bazaro.Core.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Bazaro.Application.Handlers.QueryHandlers
{
    public class GetInboxUsersHandler : IRequestHandler<GetInboxUsersQuery, List<Tuple<string, InboxUserModel>>>
    {
        private readonly IInboxRepository _inboxRepository;

        public GetInboxUsersHandler(IInboxRepository inboxRepository)
        {
            _inboxRepository = inboxRepository;
        }

        public async Task<List<Tuple<string, InboxUserModel>>> Handle(GetInboxUsersQuery request, CancellationToken cancellationToken)
        {
            var Result = await _inboxRepository.GetInboxUsersAsync(request.InboxId);
            return Result.Select(p => new Tuple<string, InboxUserModel>(p.Item1, InboxUserMapper.Mapper.Map<InboxUserModel>(p.Item2))).ToList();
        }
    }
}
