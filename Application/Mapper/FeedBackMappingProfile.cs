using AutoMapper;
using Bazaro.Application.Responses;
using Bazaro.Core.Entities;

namespace Bazaro.Application.Mapper
{
    public class FeedBackMappingProfile : Profile
    {
        public FeedBackMappingProfile()
        {
            CreateMap<InboxFeedBack, FeedBackResponse>().ReverseMap();
        }
    }
}
