using AutoMapper;
using CMPNatural.Application.Responses;
using CMPNatural.Core.Entities;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Mapper
{
    public class DriverMapperProfile : Profile
    {
        public DriverMapperProfile()
        {
            CreateMap<Driver, DriverResponse>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Person.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Person.LastName))
                .ReverseMap()
                .ForPath(src => src.Person.FirstName, opt => opt.Ignore())
                .ForPath(src => src.Person.LastName, opt => opt.Ignore());
        }
    }
}
