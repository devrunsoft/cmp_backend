using AutoMapper;
using Bazaro.Application.Responses;
using Bazaro.Core.Entities;

namespace Bazaro.Application.Mapper
{
    public class NotificationMappingProfile : Profile
    {
        public NotificationMappingProfile()
        {
            CreateMap<NotificationSystem, NotificationResponse>()
                //.ForMember(x => x.Date, opt => opt.MapFrom(src => src.Date.ToShortDateString()))
              .ReverseMap();
        }
    }
}
