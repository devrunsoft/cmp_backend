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
    public class GetAllBannerAdsHandler : IRequestHandler<GetAllBannerAdsQuery, List<BannerAdsResponse>>
    {
        private readonly IBannerAdRepository _bannerAdRepository;

        public GetAllBannerAdsHandler(IBannerAdRepository bannerAdRepository)
        {
            _bannerAdRepository = bannerAdRepository;
        }

        public async Task<List<BannerAdsResponse>> Handle(GetAllBannerAdsQuery request, CancellationToken cancellationToken)
        {
            var banners = await _bannerAdRepository.GetAllAsync();
            return banners.Select(p => BannerAdsMapper.Mapper.Map<BannerAdsResponse>(p)).ToList();
        }
    }
}
