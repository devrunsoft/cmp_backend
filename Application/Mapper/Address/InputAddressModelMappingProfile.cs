using AutoMapper;
using Bazaro.Core.Entities;
using Bazaro.Core.Models;

namespace Bazaro.Application.Mapper
{
    public class InputAddressModelMappingProfile : Profile
    {
        public InputAddressModelMappingProfile()
        {
            CreateMap<InputAddressModel, Address>().ReverseMap();
        }
    }
}
