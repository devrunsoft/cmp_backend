using AutoMapper;
using Bazaro.Application.Responses;
using Bazaro.Core.Entities;

namespace Bazaro.Application.Mapper
{
    public class PaymentMappingProfile : Profile
    { 
        private object getPrice(Payment src)
        {
            if (src.TariffTypeId == 1) // fixed
                return src.Tariff?.FixedTariff?.Price;
            else
                return src.Tariff?.Commission?.MaxPrice;
        }

        public PaymentMappingProfile()
        {
            CreateMap<Payment, PaymentResponse>()
                .ForMember(x => x.TarrifType, opt => opt.MapFrom(src => src.TariffTypeId))
                .ForMember(x => x.Title, opt => opt.MapFrom(src => src.Tariff.Title))
                .ForMember(x => x.Date, opt => opt.MapFrom(src => src.PaymentDate)) 
                .ForMember(x => x.Price, opt => opt.MapFrom(src => getPrice(src)))
                .ForMember(x => x.PaymentType, opt => opt.MapFrom(src => src.PaymentType))
                .ForMember(x => x.PaymentStatus, opt => opt.MapFrom(src => src.PaymentStatus))
                .ReverseMap();
        }
    }
}
