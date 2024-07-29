using Bazaro.Application.Responses.Base;
using MediatR;

namespace Bazaro.Application.Commands
{
    public class SetDeliveryCommand : IRequest<CommandResponse>
    {
        public int DeliveryId { get; set; }
        public int? ShopId { get; set; }
    }
}
