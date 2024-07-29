using Bazaro.Application.Responses.Base;
using MediatR;

namespace Bazaro.Application.Commands
{
    public class SetShopOwnerCommand : IRequest<CommandResponse>
    {
        public long PersonId { get; set; }
        public int ShopId { get; set; }
    }
}
