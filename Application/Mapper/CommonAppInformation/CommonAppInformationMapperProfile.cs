using AutoMapper;
using CMPNatural.Application.Model;
using CMPNatural.Application.Responses;
using CMPNatural.Application.Responses.ProviderServiceAssignment;
using CMPNatural.Core.Entities;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Mapper
{
    public class CommonAppInformationMapperProfile : Profile
    {
        public CommonAppInformationMapperProfile()
        {
            CreateMap<CommonInformationModel, AppInformation>()
                .ReverseMap();
        }
    }
}
