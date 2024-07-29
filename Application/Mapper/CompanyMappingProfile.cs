using AutoMapper;
using CMPNatural.Application.Responses;
using CMPNatural.Core.Entities;

namespace CMPNatural.Application.Mapper
{
    public class CompanyMappingProfile : Profile
    {
        public CompanyMappingProfile()
        {
            CreateMap<Company, CompanyResponse>().ReverseMap();
        }
    }
}
