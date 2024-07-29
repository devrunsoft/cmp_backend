using AutoMapper;
using Bazaro.Application.Commands;
using Bazaro.Core.Models;

namespace Bazaro.Application.Mapper
{
    public class CreateAddressMappingProfile : Profile
    {
        public CreateAddressMappingProfile()
        {
            CreateMap<InputAddressModel, CreateAddressCommand>();
        }
    }
}
