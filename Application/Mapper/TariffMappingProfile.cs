using AutoMapper;
using Bazaro.Application.Responses;
using Bazaro.Core.Entities;

namespace Bazaro.Application.Mapper
{
    public class TariffMappingProfile : Profile
    {
        public TariffMappingProfile()
        {
            CreateMap<Tariff, TariffResponse>()
                 .ForMember(x => x.Commission, opt => opt.MapFrom(src => new TarrifCommissionResponse()
                 {
                     Id = src.Commission.Id,
                     Percent = src.Commission.Percent,
                     MaxPrice = src.Commission.MaxPrice,
                     Descrtiption = src.Commission.Descrtiption
                 }))
                  .ForMember(x => x.FixedTariff, opt => opt.MapFrom(src => new FixedTariffResponse()
                  {
                      Id = src.FixedTariff.Id,
                      Day = src.FixedTariff.Day,
                      Price = src.FixedTariff.Price,
                  }))
                .ReverseMap();
        }
    }
}
