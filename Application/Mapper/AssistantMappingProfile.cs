using AutoMapper;
using Bazaro.Application.Responses;
using Bazaro.Core.Entities;

namespace Bazaro.Application.Mapper
{
    public class AssistantMappingProfile : Profile
    {
        public AssistantMappingProfile()
        {
            CreateMap<ShopUser, AssistantResponse>()
                 .ForMember(x => x.PersonInfo, opt => opt.MapFrom(src => new PersonResponse()
                 {
                     Name = src.Person.Name,
                     Mobile = src.Person.Mobile,
                     Logo = src.Person.Logo,
                     Age = src.Person.Age
                 })).ReverseMap();
        }
    }
}
