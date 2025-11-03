// CMPNatural.Application/Responses/ManifestFlatResponse.cs
using System;
using System.Collections.Generic;
using CMPNatural.Core.Enums;

namespace CMPNatural.Application.Responses
{
    public class ManifestResponse
    {
        public long Id { get; set; }
        public ManifestStatus Status { get; set; }
        public string ManifestNumber { get; set; } = "";
        public DateTime? ServiceDateTime { get; set; }

        // Manifest-related extras
        public string Number { get; set; } = "";
        public string NoteTitle { get; set; } = "";
        public string? ProviderName { get; set; }
        public long? ProviderId { get; set; }
        public string? DriverFullName { get; set; }

        // Company (from Invoice.Company)
        public string? CompanyPrimaryFirstName { get; set; }
        public string? CompanyPrimaryLastName { get; set; }
        public string? CompanyPrimaryPhoneNumber { get; set; }

        public long CompanyId { get; set; }

        // Operational Address (from Invoice.OperationalAddress)
        public long OperationalAddressAddressId { get; set; }
        public string? OperationalAddressAddress { get; set; }
        public string? OperationalAddressLocationPhone { get; set; }

        // Billing Info (from Invoice.BillingInformation)
        public string? BillingAddress { get; set; }
        public string? BillingCity { get; set; }
        public string? BillingState { get; set; }
        public string? BillingZipCode { get; set; }

        // Services (from Invoice.BaseServiceAppointments)
        public BaseServiceAppointmentFlatResponse Services { get; set; } = new();
        public ServiceAppointmentLocationResponse ServiceAppointmentLocations { get; set; } = new();


        public string? Comment { get; set; } // Invoice.Comment
    }

    public class BaseServiceAppointmentFlatResponse
    {
        public ProductMinResponse? Product { get; set; }
        public ProductPriceMinResponse? ProductPrice { get; set; }
        public DateTime? StartDate { get; set; }
        public string? DayOfWeek { get; set; }
        public int? FromHour { get; set; }
        public int? ToHour { get; set; }
    }

    public class ProductMinResponse { public string? Name { get; set; } }
    public class ProductPriceMinResponse { public string? Name { get; set; } }

    public class ServiceAppointmentLocationResponse
    {
        public LocationCompanyMinResponse? LocationCompany { get; set; }
        public decimal? Qty { get; set; }
        public decimal? FactQty { get; set; }
        public string OilQuality { get; set; } // match FE enum/int
    }

    public class LocationCompanyMinResponse
    {
        public string? Name { get; set; }
        public string? Address { get; set; }
    }
}