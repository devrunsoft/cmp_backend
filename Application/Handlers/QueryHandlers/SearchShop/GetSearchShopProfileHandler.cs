using Bazaro.Application.Mapper;
using Bazaro.Application.Queries;
using Bazaro.Application.Responses;
using Bazaro.Core.Repositories;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Bazaro.Application.Handlers.QueryHandlers
{
    public class GetSearchShopProfileHandler : IRequestHandler<GetSearchShopProfileQuery, SearchShopProfileResponse>
    {
        private readonly IShopInfoRepository _shopInfoRepository;

        public GetSearchShopProfileHandler(IShopInfoRepository shopInfoRepository)
        {
            _shopInfoRepository = shopInfoRepository;
        }

        public async Task<SearchShopProfileResponse> Handle(GetSearchShopProfileQuery request, CancellationToken cancellationToken)
        {
            var shopProfile = await _shopInfoRepository.GetShopProfileByIdAsync(request.ShopId);
            return SearchShopProfileMapper.Mapper.Map<SearchShopProfileResponse>(shopProfile);
        }
    }
}
