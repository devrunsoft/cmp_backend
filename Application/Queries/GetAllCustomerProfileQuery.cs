using Bazaro.Application.Responses;
using Bazaro.Core.Base;
using MediatR;
using System.Collections.Generic;

namespace Bazaro.Application.Queries
{
    public class GetAllCustomerProfileQuery : PagedQueryRequest, IRequest<List<CustomerProfileResponse>>
    {
        //public GetAllCustomerProfileQuery(PagedQueryRequest def)
        //{
        //    Page = def.Page;
        //    Size = def.Size;
        //}
        public int ShopId { get; set; }
    }
}
