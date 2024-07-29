using AutoMapper;
using Bazaro.Application.Responses;
using Bazaro.Core.Entities;

namespace Bazaro.Application.Mapper
{
    public class ShopTypeMappingProfile : Profile
    {
        public ShopTypeMappingProfile()
        {
            CreateMap<ShopType, ShopTypeResponse>().ReverseMap();
        }
    }
}
