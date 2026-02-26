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

            CreateMap<RequestEntity, InvoiceResponse>()
                .ForMember(x => x.Provider, opt => opt.MapFrom(src => src.Provider))
                .ForMember(x => x.Company, opt => opt.MapFrom(src => src.Company))
                .ForMember(x => x.BillingInformation, opt => opt.MapFrom(src => src.BillingInformation))
                .ForMember(x => x.BaseServiceAppointment, opt => opt.MapFrom(src => src.BaseServiceAppointment))
                .ForMember(x => x.ContractId, opt => opt.MapFrom(src => src.ContractId.HasValue ? (int?)src.ContractId.Value : null))
                .ForMember(x => x.InvoiceId, opt => opt.MapFrom(src => src.RequestNumber))
                .ForMember(x => x.InvoiceNumber, opt => opt.MapFrom(src => src.RequestNumber));
        }
    }
}
