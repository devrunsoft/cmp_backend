using AutoMapper;
using CMPNatural.Application.Responses;
using CMPNatural.Core.Entities;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Mapper
{
    public class InvoiceMapperProfile : Profile
    {
        public InvoiceMapperProfile()
        {
            CreateMap<Invoice, InvoiceResponse>()
                .ForMember(x => x.Provider, opt => opt.MapFrom(src => src.Provider))
                .ReverseMap();
        }
    }
}
