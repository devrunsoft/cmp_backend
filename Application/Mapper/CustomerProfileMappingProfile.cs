using AutoMapper;
using Bazaro.Application.Responses;
using Bazaro.Core.Entities;

namespace Bazaro.Application.Mapper
{
    public class CustomerProfileMappingProfile : Profile
    {
        public CustomerProfileMappingProfile()
        {
            CreateMap<Customer, CustomerProfileResponse>()
              .ForMember(x => x.PersonInfo, opt => opt.MapFrom(src => new PersonResponse()
              {
                  Name = src.IdNavigation.Name,
                  Mobile = src.IdNavigation.Mobile,
                  Logo = src.IdNavigation.Logo,
                  Age = src.IdNavigation.Age,
              })).ReverseMap();
        }
    }
}
