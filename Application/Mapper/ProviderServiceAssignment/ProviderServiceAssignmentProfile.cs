using AutoMapper;
using CMPNatural.Application.Responses;
using CMPNatural.Application.Responses.ProviderServiceAssignment;
using CMPNatural.Core.Entities;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Mapper
{
    public class ProviderServiceAssignmentProfile : Profile
    {
        public ProviderServiceAssignmentProfile()
        {
            CreateMap<ProviderServiceAssignment, ProviderServiceAssignmentResponse>()
                .ForMember(x => x.Invoice, opt => opt.MapFrom(src => InvoiceMapper.Mapper.Map<InvoiceResponse>(src.Invoice)))
                .ForMember(x => x.Company, opt => opt.MapFrom(src => CompanyMapper.Mapper.Map<CompanyResponse>(src.Company)))
                .ReverseMap();
        }
    }
}
