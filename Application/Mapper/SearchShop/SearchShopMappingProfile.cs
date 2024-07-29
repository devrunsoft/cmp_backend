using AutoMapper;
using Bazaro.Application.Responses;
using Bazaro.Core.Entities;
using System;
using System.Linq;

namespace Bazaro.Application.Mapper
{
    public class SearchShopMappingProfile : Profile
    {
        private int GetOrderMInPrice(ShopInfo src) => src.OrderMinPrice?.Price ?? 0;
        private int GetDeliveryPrice(ShopInfo src) => src.Delivery?.Price ?? 0;

        private int GetDiscounts(ShopInfo src)
        {
            if (src.IdNavigation?.Discounts?.Any() == true)
                return src.IdNavigation.Discounts.Max(p => p.Percent);
            else
                return 0;
        }

        public SearchShopMappingProfile()
        {
            CreateMap<ShopInfo, SearchShopResponse>()
                .ForMember(x => x.OrderMinPrice, opt => opt.MapFrom(src => GetOrderMInPrice(src)))
                .ForMember(x => x.DeliveryPrice, opt => opt.MapFrom(src => GetDeliveryPrice(src)))
                .ForMember(x => x.MaxDiscountPercent, opt => opt.MapFrom(src => GetDiscounts(src)))
                //.ForMember(x => x.Distance, opt => opt.MapFrom(src =>  src.Street)) 
                .ReverseMap();
        }


    }
}
