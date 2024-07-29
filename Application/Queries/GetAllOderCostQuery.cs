using Bazaro.Application.Responses;
using Bazaro.Core.Base;
using MediatR;
using System.Collections.Generic;

namespace Bazaro.Application.Queries
{
    public class GetAllOderCostQuery : PagedQueryRequest, IRequest<List<OrderCostResponse>>
    {
        //public GetAllOderCostQuery(PagedQueryRequest def)
        //{
        //    def.Page = Page;
        //    def.Size = Size;
        //}

        public int ShopId { get; set; }
    }
}
