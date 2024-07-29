using AutoMapper;
using Bazaro.Application.Commands;
using Bazaro.Core.Entities;
using Bazaro.Core.Models;

namespace Bazaro.Application.Mapper
{
    public class CreateInboxCommandMappingProfile : Profile
    {
        public CreateInboxCommandMappingProfile()
        {
            CreateMap<InputInboxModel, CreateOrOpenInboxCommand>();
            CreateMap<CreateOrOpenInboxCommand, Inbox>().ReverseMap();
        }
    }
}
