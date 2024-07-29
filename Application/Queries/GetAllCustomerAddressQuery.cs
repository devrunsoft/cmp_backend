using Bazaro.Application.Responses;
using Bazaro.Core.Base;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazaro.Application.Queries
{
    public class GetAllCustomerAddressQuery : PagedQueryRequest, IRequest<List<CustomerAddressResponse>>
    {
        //public GetAllCustomerAddressQuery(PagedQueryRequest def)
        //{
        //    Page = def.Page;
        //    Size = def.Size;
        //}
        public int ShopId { get; set; }
    }
}
