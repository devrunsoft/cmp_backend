using Bazaro.Application.Responses;
using Bazaro.Core.Base;
using MediatR;
using System.Collections.Generic;

namespace Bazaro.Application.Queries
{
    public class GetAllFeedBackQuery : PagedQueryRequest, IRequest<List<FeedBackResponse>>
    {
        public string FormKey { get; set; }
    }
}
