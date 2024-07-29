using Bazaro.Application.Responses;
using Bazaro.Core.Base;
using MediatR;
using System.Collections.Generic;

namespace Bazaro.Application.Queries
{
    public class GetAllChatDiscountQuery : PagedQueryRequest, IRequest<List<ChatDiscountResponse>>
    {
        //public GetAllChatDiscountQuery(PagedQueryRequest def)
        //{
        //    Page = def.Page;
        //    Size = def.Size;
        //}
        public int ShopId { get; set; }
    }
}
