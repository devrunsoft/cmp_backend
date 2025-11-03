using System.Linq;
using AutoMapper;
using CMPNatural.Application.Responses;
using CMPNatural.Core.Entities;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Mapper
{
    public class RequestMapperProfile : Profile
    {
        public RequestMapperProfile()
        {
            CreateMap<RequestEntity, RequestResponse>()
                .ForMember(x => x.Provider, opt => opt.MapFrom(src => src.Provider))
                .ForMember(x => x.Company, opt => opt.MapFrom(src => src.Company))
                .ForMember(dest => dest.BaseServiceAppointment, opt => opt.MapFrom(src => src.BaseServiceAppointment))
                .ReverseMap();
        }
    }
}
