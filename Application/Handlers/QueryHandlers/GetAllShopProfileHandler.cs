using Bazaro.Application.Queries;
using Bazaro.Application.Responses;
using Bazaro.Core.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Bazaro.Application.Handlers.QueryHandlers
{
    public class GetAllShopProfileHandler : IRequestHandler<GetAllShopProfileQuery, ShopProfileResponse>
    {
        private readonly IShopInfoRepository _shopInfoRepository;
        private readonly IPersonRepository _personRepository;

        public GetAllShopProfileHandler(IShopInfoRepository shopInfoRepository, IPersonRepository personRepository)
        {
            _shopInfoRepository = shopInfoRepository;
            _personRepository = personRepository;
        }

        public async Task<ShopProfileResponse> Handle(GetAllShopProfileQuery request, CancellationToken cancellationToken)
        {
            var shopInfo = await _shopInfoRepository.GetShopInfoByIdAsync((int)request.ShopId);
            var personInfo = await _personRepository.GetByIdAsync((int)request.PersonId);

            return new ShopProfileResponse()
            {
                Id = shopInfo.Id,
                Logo = shopInfo.Logo,
                Name = shopInfo.Name,
                Mobile = personInfo.Mobile,
                Gender = personInfo.Gender,
                BirthDay = personInfo.Age,
            };

            //PaymentMapper.Mapper.Map<PaymentResponse>(Result);
        }
    }
}

