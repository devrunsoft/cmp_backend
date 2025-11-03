using AutoMapper;
using CMPNatural.Application.Responses;
using CMPNatural.Application.Responses.ProviderServiceAssignment;
using CMPNatural.Core.Entities;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Mapper
{
    public class BaseServiceAppointmentMapperProfile : Profile
    {
        public BaseServiceAppointmentMapperProfile()
        {
            CreateMap<BaseServiceAppointment, BaseServiceAppointmentResponse>()
                //.ForMember(x => x.Invoice, opt => opt.MapFrom(src => InvoiceMapper.Mapper.Map<InvoiceResponse>(src.Invoice)))
                .ReverseMap();
        }
    }
}
