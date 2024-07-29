using Bazaro.Application.Responses;
using Bazaro.Core.Base;
using MediatR;
using System.Collections.Generic;

namespace Bazaro.Application.Queries
{
    public class GetAllSupportCategoryQuery : PagedQueryRequest, IRequest<List<SupportCategoryResponse>>
    {
        public int? ParentId { get; set; }
        public bool? IsShop { get; set; }
    }
}
