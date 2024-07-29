using AutoMapper;
using Bazaro.Application.Responses;
using Bazaro.Core.Entities;

namespace Bazaro.Application.Mapper
{
    public class DiscountPriceMappingProfile : Profile
    {
        public DiscountPriceMappingProfile()
        {
            CreateMap<DiscountPrice, DiscountPriceResponse>().ReverseMap();
        }
    }
}
