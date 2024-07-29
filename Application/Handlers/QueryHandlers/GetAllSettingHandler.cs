using Bazaro.Application.Queries;
using Bazaro.Core.Entities;
using Bazaro.Core.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Bazaro.Application.Handlers.QueryHandlers
{
    public class GetAllSettingHandler : IRequestHandler<GetAllSettingQuery, SettingResponse>
    {
        private readonly IShopInfoRepository _shopInfoRepository;
        //private readonly IShopUserRepository _shopUserRepository;
        private readonly IPersonRepository _personRepository;

        public GetAllSettingHandler(IShopInfoRepository shopInfoRepository, IPersonRepository personRepository)
        {
            _shopInfoRepository = shopInfoRepository;
            _personRepository = personRepository;
        }

        public async Task<SettingResponse> Handle(GetAllSettingQuery request, CancellationToken cancellationToken)
        {
            var shopInfo = await _shopInfoRepository.GetShopInfoByIdAsync(request.ShopId);
            var personInfo = await _personRepository.GetByIdAsync(request.PersonId);

            return await Task.FromResult(new SettingResponse()
            {
                Id = shopInfo.Id,
                OrderMinPriceId = shopInfo.OrderMinPriceId,
                OrderMinPriceTitle = shopInfo.OrderMinPrice?.Title,
                DeliveryId = shopInfo.DeliveryId,
                DeliveryTitle = shopInfo.Delivery?.Title,
                Name = shopInfo.Name,
                Title = shopInfo.Title,
                Logo = shopInfo.Logo,
                IsEnable = shopInfo.IdNavigation.IsEnable,
                Number = personInfo.Mobile,
                Revenue =  "", //shopInfo.,
                ReadOnly = shopInfo.IdNavigation?.OwnerId != request.PersonId
            });
        }
    }
}
