using System.Linq;
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
                .ForMember(x => x.InvoiceProduct, opt => opt.MapFrom(src => src.InvoiceProduct))
                .ForMember(x => x.Company, opt => opt.MapFrom(src => src.Company))
                 //.ForMember(
                 //x => x.BaseServiceAppointment,
                 //opt => opt.MapFrom(src => src.ServiceAppointmentLocation.Select(sal => sal.ServiceAppointment))
                 //   )
                .ReverseMap();
        }
    }
}
