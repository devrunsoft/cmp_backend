using AutoMapper;
using Bazaro.Application.Responses;
using Bazaro.Core.Entities;

namespace Bazaro.Application.Mapper
{
    public class AddressMappingProfile : Profile
    {
        public AddressMappingProfile()
        {
            CreateMap<Address, AddressResponse>()
                .ForMember(x => x.Address, opt => opt.MapFrom(src => $"{src.Street} , {src.BuildingNumber} , {src.Unit}"))
                .ReverseMap();
        }
    }
}
