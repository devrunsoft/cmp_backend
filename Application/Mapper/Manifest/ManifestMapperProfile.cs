// CMPNatural.Application/Mapper/ManifestFlatMapperProfile.cs
using AutoMapper;
using CMPNatural.Application.Responses;
using CMPNatural.Core.Entities;

namespace CMPNatural.Application.Mapper
{
    public class ManifestMapperProfile : Profile
    {
        public ManifestMapperProfile()
        {
            // Leaves used by Services
            CreateMap<Product, ProductMinResponse>();
            CreateMap<ProductPrice, ProductPriceMinResponse>();
            CreateMap<LocationCompany, LocationCompanyMinResponse>();

            CreateMap<ServiceAppointmentLocation, ServiceAppointmentLocationResponse>()
                .ForMember(d => d.Qty, o => o.MapFrom(s => s.Qty))
                .ForMember(d => d.FactQty, o => o.MapFrom(s => s.FactQty))
                .ForMember(d => d.OilQuality, o => o.MapFrom(s => s.OilQuality.GetDescription()))
                .ForMember(d => d.LocationCompany, o => o.MapFrom(s => s.LocationCompany));

            CreateMap<BaseServiceAppointment, BaseServiceAppointmentFlatResponse>()
                .ForMember(d => d.Product, o => o.MapFrom(s => s.Product))
                .ForMember(d => d.ProductPrice, o => o.MapFrom(s => s.ProductPrice))
                .ForMember(d => d.StartDate, o => o.MapFrom(s => s.StartDate))
                .ForMember(d => d.DayOfWeek, o => o.MapFrom(s => s.DayOfWeek))
                .ForMember(d => d.FromHour, o => o.MapFrom(s => s.FromHour))
                .ForMember(d => d.ToHour, o => o.MapFrom(s => s.ToHour));

            // Root: Manifest → flat response (lifting Invoice fields)
            CreateMap<Manifest, ManifestResponse>()
                // manifest basics
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.Status, o => o.MapFrom(s => s.Status))
                .ForMember(d => d.ManifestNumber, o => o.MapFrom(s => s.ManifestNumber))
                .ForMember(d => d.ServiceDateTime, o => o.MapFrom(s => s.ServiceDateTime))
                .ForMember(d => d.Number, o => o.MapFrom(s => s.Number))
                .ForMember(d => d.NoteTitle, o => o.MapFrom(s => s.NoteTitle))
                .ForMember(d => d.ProviderName, o => o.MapFrom(s => s.Provider != null ? s.Provider.Name : null))
                .ForMember(d => d.ProviderId, o => o.MapFrom(s => s.ProviderId))
                .ForMember(d => d.DriverFullName, o => o.MapFrom(s => ""))

                // Company (via Invoice)
                .ForMember(d => d.CompanyPrimaryFirstName, o => o.MapFrom(s => s.Request.Company.PrimaryFirstName))
                .ForMember(d => d.CompanyId, o => o.MapFrom(s => s.Request.Company.Id))
                .ForMember(d => d.CompanyPrimaryLastName, o => o.MapFrom(s => s.Request.Company.PrimaryLastName))
                .ForMember(d => d.CompanyPrimaryPhoneNumber, o => o.MapFrom(s => s.Request.Company.PrimaryPhonNumber))

                // Operational Address
                .ForMember(d => d.OperationalAddressAddress, o => o.MapFrom(s => s.Request.OperationalAddress.Address))
                .ForMember(d => d.OperationalAddressLocationPhone, o => o.MapFrom(s => s.Request.OperationalAddress.LocationPhone))
                .ForMember(d => d.OperationalAddressAddressId, o => o.MapFrom(s => s.Request.OperationalAddress.Id))

                // Billing
                .ForMember(d => d.BillingAddress, o => o.MapFrom(s => s.Request.BillingInformation.Address))
                .ForMember(d => d.BillingCity, o => o.MapFrom(s => s.Request.BillingInformation.City))
                .ForMember(d => d.BillingState, o => o.MapFrom(s => s.Request.BillingInformation.State))
                .ForMember(d => d.BillingZipCode, o => o.MapFrom(s => s.Request.BillingInformation.ZIPCode))

                // Services and comment (from Invoice)
                .ForMember(d => d.Services, o => o.MapFrom(s => s.ServiceAppointmentLocation.ServiceAppointment))
                .ForMember(d => d.ServiceAppointmentLocations, o => o.MapFrom(s => s.ServiceAppointmentLocation))
                .ForMember(d => d.Comment, o => o.MapFrom(s => s.Request.Comment))

                // we don’t map back
                .ReverseMap()
                .ForPath(s => s.Request, o => o.Ignore())
                .ForPath(s => s.Provider, o => o.Ignore())
                .ForPath(s => s.DriverManifest, o => o.Ignore());
        }
    }
}