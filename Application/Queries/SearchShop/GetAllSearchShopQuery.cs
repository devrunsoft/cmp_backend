using Bazaro.Application.Responses;
using Bazaro.Core.Base;
using MediatR;
using System.Collections.Generic;

namespace Bazaro.Application.Queries
{
    public class GetAllSearchShopQuery : PagedQueryRequest, IRequest<List<SearchShopResponse>>
    {
        public int ShopTypeId { get; set; }
        public int AddressId { get; set; }
    }
}
