using AutoMapper;
using Bazaro.Application.Responses;
using Bazaro.Core.Entities;

namespace Bazaro.Application.Mapper
{
    public class DiscountPercentMappingProfile : Profile
    {
        public DiscountPercentMappingProfile()
        {
            CreateMap<DiscountPercent, DiscountPercentResponse>().ReverseMap();
        }
    }
}
