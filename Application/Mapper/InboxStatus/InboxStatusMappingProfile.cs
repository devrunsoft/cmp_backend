using AutoMapper;
using Bazaro.Application.Responses;
using Bazaro.Core.Entities;

namespace Bazaro.Application.Mapper
{
    public class InboxStatusMappingProfile : Profile
    {
        public InboxStatusMappingProfile()
        {
            CreateMap<InboxStatus, InboxStatusResponse>().ReverseMap();
        }
    }
}
