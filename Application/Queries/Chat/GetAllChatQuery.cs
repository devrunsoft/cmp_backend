using Bazaro.Application.Responses;
using Bazaro.Core.Base;
using MediatR;
using System.Collections.Generic;

namespace Bazaro.Application.Queries
{
    public class GetAllChatQuery : PagedQueryRequest, IRequest<List<ChatResponse>>
    {
        public long InboxId { get; set; }        
        public int ShopId { get; set; }
    }
}
