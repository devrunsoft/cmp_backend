using Bazaro.Application.Responses;
using MediatR;
using System.Collections.Generic;

namespace Bazaro.Application.Queries
{
    public class GetAllDiscountQuery : IRequest<List<DiscountResponse>>
    {
        public int? ShopId { get; set; }
    }
}
