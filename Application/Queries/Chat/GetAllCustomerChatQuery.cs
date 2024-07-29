using Bazaro.Application.Responses;
using Bazaro.Core.Base;
using MediatR;
using System.Collections.Generic;

namespace Bazaro.Application.Queries
{
    public class GetAllCustomerChatQuery : PagedQueryRequest, IRequest<List<ChatResponse>>
    {
        public long InboxId { get; set; }
        public long PersonId { get; set; }
    }
}
