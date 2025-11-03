using AutoMapper;
using CMPNatural.Application.Responses;
using CMPNatural.Application.Responses.Driver;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Enums;

namespace CMPNatural.Application.Mapper
{
    public class RouteLocationMappingProfile : Profile
    {
        public RouteLocationMappingProfile()
        {
            // Parent model
            CreateMap<RouteServiceAppointmentLocation, RouteLocationResponse>()
                .ForMember(d => d.Id, m => m.MapFrom(s => s.Id))
                .ForMember(d => d.RouteId, m => m.MapFrom(s => s.RouteId))
                .ForMember(d => d.ManifestNumber, m => m.MapFrom(s => s.ManifestNumber))
                .ForMember(d => d.LocationCompanyId, m => m.MapFrom(s => s.ServiceAppointmentLocation!.LocationCompany!.Id))
                .ForMember(d => d.Address, m => m.MapFrom(s => s.ServiceAppointmentLocation!.LocationCompany!.Address))
                .ForMember(d => d.PrimaryFirstName, m => m.MapFrom(s => s.ServiceAppointmentLocation!.LocationCompany!.PrimaryFirstName))
                .ForMember(d => d.PrimaryLastName, m => m.MapFrom(s => s.ServiceAppointmentLocation!.LocationCompany!.PrimaryLastName))
                .ForMember(d => d.PrimaryPhonNumber, m => m.MapFrom(s => s.ServiceAppointmentLocation!.LocationCompany!.PrimaryPhonNumber))
                .ForMember(d => d.CompanyName, m => m.MapFrom(s => s.ServiceAppointmentLocation!.LocationCompany!.Company!.CompanyName))
                .ForMember(d => d.Lat, m => m.MapFrom(s => s.ServiceAppointmentLocation!.LocationCompany!.Lat))
                .ForMember(d => d.Lng, m => m.MapFrom(s => s.ServiceAppointmentLocation!.LocationCompany!.Long))
                .ForMember(d => d.Status, m => m.MapFrom(s => s.ServiceAppointmentLocation!.Status))
                .ForMember(d => d.Services, m => m.MapFrom(s => s)); // map with the nested map below

            // Nested "Services" block
            CreateMap<RouteServiceAppointmentLocation, RouteServices>()
                .ForMember(d => d.ProductName, m => m.MapFrom(s => s.ServiceAppointmentLocation!.ServiceAppointment!.Product!.Name))
                .ForMember(d => d.ProductPriceName, m => m.MapFrom(s => s.ServiceAppointmentLocation!.ServiceAppointment!.ProductPrice!.Name))
                .ForMember(d => d.IsEmegency, m => m.MapFrom(s => s.ServiceAppointmentLocation!.ServiceAppointment!.IsEmegency))
                .ForMember(d => d.Capacity, m => m.MapFrom(s => s.ServiceAppointmentLocation!.ServiceAppointment!.Qty))
                .ForMember(d => d.Status, m => m.MapFrom(s => s.ServiceAppointmentLocation!.Status))
                .ForMember(d => d.FinishDate, m => m.MapFrom(s => s.ServiceAppointmentLocation!.FinishDate))
                .ForMember(d => d.StartedAt, m => m.MapFrom(s => s.ServiceAppointmentLocation!.StartedAt))
                .ForMember(d => d.ServiceAppointmentLocationId, m => m.MapFrom(s => s.ServiceAppointmentLocation!.Id))
                .ForMember(d => d.ServiceType, m => m.MapFrom(s =>
                    ((ServiceType)s.ServiceAppointmentLocation!.ServiceAppointment!.Product.ServiceType).GetDescription()

                ));
        }
    }
}