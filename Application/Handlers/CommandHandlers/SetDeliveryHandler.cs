using Bazaro.Application.Commands;
using Bazaro.Application.Responses.Base;
using Bazaro.Core.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Bazaro.Application.Handlers.CommandHandlers
{
    public class SetDeliveryHandler : IRequestHandler<SetDeliveryCommand, CommandResponse>
    {
        private readonly IShopInfoRepository _shopInfoRepository;

        public SetDeliveryHandler(IShopInfoRepository shopInfoRepository)
        {
            _shopInfoRepository = shopInfoRepository;
        }

        public async Task<CommandResponse> Handle(SetDeliveryCommand request, CancellationToken cancellationToken)
        {
            var shopInfo = await _shopInfoRepository.GetByIdAsync((int)request.ShopId);

            if (shopInfo == null)
                return new ResponseNotFound();

            shopInfo.DeliveryId = request.DeliveryId;
            await _shopInfoRepository.UpdateAsync(shopInfo);

            return new Success() { };
        }
    }

}
