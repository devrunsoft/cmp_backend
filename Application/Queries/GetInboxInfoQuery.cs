using Bazaro.Application.Responses;
using MediatR;

namespace Bazaro.Application.Queries
{
    public class GetInboxInfoQuery : IRequest<InboxInfoResponse>
    { 
        public long Id { get; set; }
    }
}
