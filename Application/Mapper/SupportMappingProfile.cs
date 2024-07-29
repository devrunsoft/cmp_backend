using AutoMapper;
using Bazaro.Application.Responses;
using Bazaro.Core.Entities;

namespace Bazaro.Application.Mapper
{
    public class SupportMappingProfile : Profile
    {
        public SupportMappingProfile()
        {
            CreateMap<Support, SupportResponse>().ReverseMap();
        }
    }
}
