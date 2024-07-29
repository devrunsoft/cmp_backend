using Bazaro.Application.Mapper;
using Bazaro.Application.Queries;
using Bazaro.Application.Responses;
using Bazaro.Core.Repositories;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Bazaro.Application.Handlers.QueryHandlers
{
    public class GetInboxInfoHandler : IRequestHandler<GetInboxInfoQuery, InboxInfoResponse>
    {
        private readonly IInboxRepository _inboxRepo;

        public GetInboxInfoHandler(IInboxRepository inboxRepository)
        {
            _inboxRepo = inboxRepository;
        }

        public async Task<InboxInfoResponse> Handle(GetInboxInfoQuery request, CancellationToken cancellationToken)
        {
            var Result = await _inboxRepo.GetByIdAsync(request.Id);

            return new InboxInfoResponse()
            {
                Id = Result.Id,
                AddressId = Result.AddressId,
                CustomerId = Result.CustomerId,
            }; 
        }
    }
}
