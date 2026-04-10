using AutoMapper;
using CMPNatural.Application.Responses;
using CMPNatural.Core.Entities;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Mapper
{
    public class DriverMapperProfile : Profile
    {
        public DriverMapperProfile()
        {
            CreateMap<Driver, DriverResponse>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Person.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Person.LastName))
                .ReverseMap()
                .ForPath(src => src.Person.FirstName, opt => opt.Ignore())
                .ForPath(src => src.Person.LastName, opt => opt.Ignore());

            CreateMap<ProviderDriver, DriverResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Driver.Id))
                .ForMember(dest => dest.PersonId, opt => opt.MapFrom(src => src.Driver.PersonId))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Driver.Email))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Driver.Password))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Driver.Person.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Driver.Person.LastName))
                .ForMember(dest => dest.License, opt => opt.MapFrom(src => src.Driver.License))
                .ForMember(dest => dest.LicenseExp, opt => opt.MapFrom(src => src.Driver.LicenseExp))
                .ForMember(dest => dest.BackgroundCheck, opt => opt.MapFrom(src => src.Driver.BackgroundCheck))
                .ForMember(dest => dest.BackgroundCheckExp, opt => opt.MapFrom(src => src.Driver.BackgroundCheckExp))
                .ForMember(dest => dest.ProfilePhoto, opt => opt.MapFrom(src => src.Driver.ProfilePhoto))
                .ForMember(dest => dest.ProviderId, opt => opt.MapFrom(src => src.ProviderId))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Driver.Status))
                .ForMember(dest => dest.TwoFactor, opt => opt.MapFrom(src => src.Driver.TwoFactor))
                .ForMember(dest => dest.IsDefault, opt => opt.MapFrom(src => src.IsDefault));
        }
    }
}
