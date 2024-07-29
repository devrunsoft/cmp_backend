using AutoMapper;
using Bazaro.Application.Responses;
using Bazaro.Core.Entities;

namespace Bazaro.Application.Mapper
{
    public class FixedTarrifMappingProfile : Profile
    {
        public FixedTarrifMappingProfile()
        {
            CreateMap<FixedTariff, FixedTariffResponse>()
                  .ForMember(x => x.Tariff, opt => opt.MapFrom(src => new TariffResponse()
                  {
                      Title = src.IdNavigation.Title,
                      Name = src.IdNavigation.Name,
                      TariffTypeId = src.IdNavigation.Id,
                  }))
                .ReverseMap();
        }
    }
}
