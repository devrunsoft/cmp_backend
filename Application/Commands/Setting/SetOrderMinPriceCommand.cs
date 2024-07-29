using Bazaro.Application.Responses.Base;
using MediatR;

namespace Bazaro.Application.Commands
{
    public class SetOrderMinPriceCommand : IRequest<CommandResponse>
    {
        public int? ShopId { get; set; }
        public int OrderMinPriceId { get; set; }
    }
}
