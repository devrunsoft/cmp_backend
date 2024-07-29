using Bazaro.Application.Responses.Base; 
using MediatR;

namespace Bazaro.Application.Commands
{
    public class ReloadShopInfoCacheCommand : IRequest<CommandResponse>
    {
        public int ShopId { get; set; }
    }
}
