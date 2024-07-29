using Bazaro.Application.Responses;
using MediatR;

namespace Bazaro.Application.Queries
{
    public class GetOrderMinPriceQuery : IRequest<OrderMinPriceResponse>
    {
        public int ShopId { get; set; }
    }
}
