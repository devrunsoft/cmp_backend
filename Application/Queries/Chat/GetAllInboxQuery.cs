using Bazaro.Application.Responses;
using Bazaro.Core.Base;
using MediatR;
using System.Collections.Generic;

namespace Bazaro.Application.Queries
{
    public class GetAllInboxQuery : PagedQueryRequest, IRequest<List<InboxResponse>>
    { 
        public long? Id { get; set; }
        public int? ShopId { get; set; }
    }
}
