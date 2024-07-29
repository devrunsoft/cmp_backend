using AutoMapper;
using Bazaro.Application.Responses;
using Bazaro.Core.Entities;
using System;

namespace Bazaro.Application.Mapper
{
    public class InboxCustomerMappingProfile : Profile
    {
        private object getUnreadFunc(Inbox src)
        {
            return src.LastChat?.Seen != true;
        } 

        public InboxCustomerMappingProfile()
        {
            CreateMap<Inbox, InboxResponse>()
                .ForMember(x => x.Logo, opt => opt.MapFrom(src => src.Shop.ShopInfo.Logo))
                .ForMember(x => x.Name, opt => opt.MapFrom(src => src.Shop.ShopInfo.Name))
                .ForMember(x => x.Title, opt => opt.MapFrom(src => src.Shop.ShopInfo.Title))
                .ForMember(x => x.Unread, opt => opt.MapFrom(src => getUnreadFunc(src)))

                .ForMember(x => x.StatusCode, opt => opt.MapFrom(src => new InboxStatusResponse()
                {
                    Id = src.InboxStatus.Id,
                    ShopTitleStatus = src.InboxStatus.ShopTitleStatus,
                    ShopNameStatus = src.InboxStatus.ShopNameStatus,
                    CustomerTitleStatus = src.InboxStatus.CustomerTitleStatus,
                    CustomerNameStatus = src.InboxStatus.CustomerNameStatus,
                    TextOnChat = src.InboxStatus.TextOnChat,
                    Description = src.InboxStatus.Description,

                }))
                 .ForMember(x => x.LastChat, opt => opt.MapFrom(src => new ChatResponse()
                 {
                     Id = src.LastChat.Id,
                     InboxId = src.LastChat.InboxId,
                     TypeId = src.LastChat.TypeId,
                     Body = src.LastChat.Body,
                     File = src.LastChat.File,
                     ShopUserId = src.LastChat.ShopUserId,
                     Seen = src.LastChat.Seen,
                     CreatedAt = src.LastChat.CreatedAt,
                 }))
                  .ForMember(x => x.Address, opt => opt.MapFrom(src => new AddressResponse()
                  {
                      Id = src.Address.Id,
                      CustomerId = src.Address.CustomerId,
                      Name = src.Address.Name,
                      Address = $"{src.Address.Street} , {src.Address.BuildingNumber} , {src.Address.Unit}",
                      Long = src.Address.Long,
                      Lat = src.Address.Lat,
                  }))
                .ReverseMap();
        }
    }
}
