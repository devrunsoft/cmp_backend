using AutoMapper;
using Bazaro.Application.Responses;
using Bazaro.Core.Entities;
using System;
using System.Linq.Expressions;

namespace Bazaro.Application.Mapper
{
    public class PaymentListMappingProfile : Profile
    {
        private object getPrice(Payment src)
        {
            if (src.TariffTypeId == 1) // fixed
                return src.Tariff?.FixedTariff?.Price;
            else
                return src.Tariff?.Commission?.MaxPrice;
        }

        public PaymentListMappingProfile()
        {
            CreateMap<Payment, PaymentListResponse>()
                .ForMember(x => x.TarrifType, opt => opt.MapFrom(src => src.TariffTypeId))
                .ForMember(x => x.Title, opt => opt.MapFrom(src => src.Tariff.Title))
                .ForMember(x => x.Date, opt => opt.MapFrom(src => src.PaymentDate))
             //   .ForMember(x => x.PaymentStatus, opt => opt.MapFrom(src => src.PaymentDate))
                .ForMember(x => x.Price, opt => opt.MapFrom(src => getPrice(src)))
                .ReverseMap();
        }
    }
}
