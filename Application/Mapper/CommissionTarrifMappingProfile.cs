using AutoMapper;
using Bazaro.Application.Responses;
using Bazaro.Core.Entities;

namespace Bazaro.Application.Mapper
{
    public class CommissionTarrifMappingProfile : Profile
    {
        public CommissionTarrifMappingProfile()
        {
            CreateMap<Commission, TarrifCommissionResponse>()
                 .ForMember(x => x.Tarrif, opt => opt.MapFrom(src => new TariffResponse()
                 {
                     Id = src.IdNavigation.Id,
                     Title = src.IdNavigation.Title,
                     Name = src.IdNavigation.Name,
                     TariffTypeId = src.IdNavigation.TariffTypeId,
                 }))
                .ReverseMap();
        }
    }
}
