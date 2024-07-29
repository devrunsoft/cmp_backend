using Bazaro.Application.Queries;
using Bazaro.Application.Responses.Base;
using Bazaro.Core.Repositories;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Bazaro.Application.Handlers.QueryHandlers
{
    public class DeleteRelatedInboxChacheHandler : IRequestHandler<DeleteRelatedInboxChacheCommand, CommandResponse>
    {
        private readonly IInboxRepository _inboxRepository;

        public DeleteRelatedInboxChacheHandler(IInboxRepository inboxRepository)
        {
            _inboxRepository = inboxRepository;
        }

        public async Task<CommandResponse> Handle(DeleteRelatedInboxChacheCommand request, CancellationToken cancellationToken)
        {
            var result = new List<long>();

            if (request.ShopId != null)
                result.AddRange(await _inboxRepository.GetShopInboxIdsAsync((int)request.ShopId));

            if (request.PersonId != null)
                result.AddRange(await _inboxRepository.GetPersonInboxIdsAsync((int)request.PersonId));

            if (request.ShopUserPersonId != null)
                result.AddRange(await _inboxRepository.GetShopUserInboxIdsByPersonIdAsync((int)request.ShopUserPersonId));

            if (result.Any())
            {
                _inboxRepository.RemoveInboxCachesAsync(result);
            }

            return new Success();
        }
    }
}
