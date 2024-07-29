using Bazaro.Application.Responses;
using Bazaro.Core.Base;
using MediatR;
using System.Collections.Generic;

namespace Bazaro.Application.Queries
{
    public class GetAllCustomerInboxQuery : PagedQueryRequest, IRequest<List<InboxResponse>>
    { 
        public long? Id { get; set; }
        public long? CustomerId { get; set; }
    }
}
