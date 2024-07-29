using AutoMapper;
using Bazaro.Application.Responses;
using Bazaro.Core.Entities;

namespace Bazaro.Application.Mapper
{
    public class OrderMinCostMappingProfile : Profile
    {
        public OrderMinCostMappingProfile()
        {
            CreateMap<OrderMinPrice, OrderMinPriceResponse>().ReverseMap(); 
        }
    }
}
