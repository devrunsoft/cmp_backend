using AutoMapper;
using Bazaro.Application.Responses;
using Bazaro.Core.Entities;
using System.Linq;

namespace Bazaro.Application.Mapper
{
    public class SearchShopProfileMappingProfile : Profile
    {
        OrderMinPriceResponse toOrderMinPrice(OrderMinPrice p) => OrderMinCostMapper.Mapper.Map<OrderMinPriceResponse>(p);
        DeliveryResponse toDeliveryResponse(Delivery p) => DeliveryMapper.Mapper.Map<DeliveryResponse>(p);
        DiscountResponse toDiscountResponse(Discount discount) => DiscountMapper.Mapper.Map<DiscountResponse>(discount);
        SearchShopProfileStatusResponse toSearchShopProfileStatusResponse(ShopInfo p) => new SearchShopProfileStatusResponse()
        {
            Comments = 10,
            SatisfiedCustomers = 7,
            DissatisfiedCustomers = 3,
        };
        public SearchShopProfileMappingProfile()
        {
            CreateMap<ShopInfo, SearchShopProfileResponse>()
                .ForMember(x => x.OrderMinPrice, opt => opt.MapFrom(src => toOrderMinPrice(src.OrderMinPrice)))
                .ForMember(x => x.Delivery, opt => opt.MapFrom(src => toDeliveryResponse(src.Delivery)))
                .ForMember(x => x.Status, opt => opt.MapFrom(src => toSearchShopProfileStatusResponse(src)))
                .ForMember(x => x.Discounts, opt => opt.MapFrom(src => src.IdNavigation.Discounts.Select(p => toDiscountResponse(p))))

                //.ForMember(x => x.Distance, opt => opt.MapFrom(src =>  src.Street)) 
                .ReverseMap();
        }
    }
}
