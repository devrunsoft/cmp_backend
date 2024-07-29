using AutoMapper; 
using Bazaro.Core.Entities;
using Bazaro.Core.Models;

namespace Bazaro.Application.Mapper
{
    public class InboxUserMappingProfile : Profile
    {
        public InboxUserMappingProfile()
        {
            CreateMap<Person, InboxUserModel>()
                 //.ForMember(x => x., opt => opt.MapFrom(src => new PersonResponse()
                 //{
                 //    Name = src.Person.Name,
                 //    Mobile = src.Person.Mobile,
                 //    Logo = src.Person.Logo,
                 //    Age = src.Person.Age
                 //}))
                 .ReverseMap();
        }
    }
}
