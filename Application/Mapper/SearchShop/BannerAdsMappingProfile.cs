using AutoMapper;
using Bazaro.Application.Responses;
using Bazaro.Core.Entities;

namespace Bazaro.Application.Mapper
{
    public class BannerAdsMappingProfile : Profile
    {
        public BannerAdsMappingProfile()
        {
            CreateMap<BannerAd, BannerAdsResponse>().ReverseMap();
        } 
    }
}
