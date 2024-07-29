using Bazaro.Application.Responses;
using MediatR;

namespace Bazaro.Application.Queries
{
    public class GetDeliveryCostQuery : IRequest<DeliveryResponse>
    {
        public int ShopId { get; set; }
    }
}
