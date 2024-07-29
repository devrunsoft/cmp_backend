using Bazaro.Application.Commands;
using Bazaro.Application.Responses.Base;
using Bazaro.Core.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
 
namespace Bazaro.Application.Handlers.CommandHandlers
{
    public class SetOrderMinPriceHandler : IRequestHandler<SetOrderMinPriceCommand, CommandResponse>
    { 
        
        private readonly IShopInfoRepository _shopInfoRepository;

        public SetOrderMinPriceHandler(IShopInfoRepository shopInfoRepository)
        {
            _shopInfoRepository = shopInfoRepository;
        }

        public async Task<CommandResponse> Handle(SetOrderMinPriceCommand request, CancellationToken cancellationToken)
        {
            var shopInfo = await _shopInfoRepository.GetByIdAsync((int)request.ShopId);

            if (shopInfo == null)
                return new ResponseNotFound();

            shopInfo.OrderMinPriceId = request.OrderMinPriceId;
            await _shopInfoRepository.UpdateAsync(shopInfo);

            return new Success() { };
        }
    }

}