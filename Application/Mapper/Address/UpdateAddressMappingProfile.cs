using AutoMapper;
using Bazaro.Application.Commands;
using Bazaro.Core.Models;

namespace Bazaro.Application.Mapper
{
    public class UpdateAddressMappingProfile : Profile
    {
        public UpdateAddressMappingProfile()
        {
            CreateMap<InputAddressModel, UpdateAddressCommand>();
        }
    }
}
