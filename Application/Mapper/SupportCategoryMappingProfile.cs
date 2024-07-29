using AutoMapper;
using Bazaro.Application.Responses;
using Bazaro.Core.Entities;

namespace Bazaro.Application.Mapper
{
    public class SupportCategoryMappingProfile : Profile
    {
        public SupportCategoryMappingProfile()
        {
            CreateMap<SupportCategory, SupportCategoryResponse>()
                 .ForMember(x => x.Id, opt => opt.MapFrom(src => src.Id))
                 .ForMember(x => x.Name, opt => opt.MapFrom(src => src.Name))
                 .ForMember(x => x.Title, opt => opt.MapFrom(src => src.Title))
                 .ForMember(x => x.ParentId, opt => opt.MapFrom(src => src.ParentId))
                .ReverseMap();
        }
    }
}
