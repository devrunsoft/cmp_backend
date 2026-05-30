using System;
using System.Collections.Generic;

namespace CMPNatural.Application.Model
{
    public class ProviderDatabaseExportPayload
    {
        public DateTime ExportedAtUtc { get; set; }
        public List<ProviderDbRow> Providers { get; set; } = new List<ProviderDbRow>();
        public List<PersonDbRow> Persons { get; set; } = new List<PersonDbRow>();
        public List<ProviderServiceDbRow> ProviderServices { get; set; } = new List<ProviderServiceDbRow>();
        public List<DriverDbRow> Drivers { get; set; } = new List<DriverDbRow>();
        public List<ProviderDriverDbRow> ProviderDrivers { get; set; } = new List<ProviderDriverDbRow>();
        public List<VehicleDbRow> Vehicles { get; set; } = new List<VehicleDbRow>();
        public List<ProviderVehicleDbRow> ProviderVehicles { get; set; } = new List<ProviderVehicleDbRow>();
        public List<VehicleCompartmentDbRow> VehicleCompartments { get; set; } = new List<VehicleCompartmentDbRow>();
        public List<VehicleServiceDbRow> VehicleServices { get; set; } = new List<VehicleServiceDbRow>();
    }

    public class ProviderDbRow
    {
        public long Id { get; set; }
        public Guid? PersonId { get; set; }
        public string Name { get; set; } = string.Empty;
        public double? Rating { get; set; }
        public int Status { get; set; }
        public double? Lat { get; set; }
        public double? Long { get; set; }
        public double? AreaLocation { get; set; }
        public string? City { get; set; }
        public string? Address { get; set; }
        public string? County { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string? Password { get; set; }
        public int RegistrationStatus { get; set; }
        public string? BusinessLicense { get; set; }
        public DateTime? BusinessLicenseExp { get; set; }
        public string? HealthDepartmentPermit { get; set; }
        public DateTime? HealthDepartmentPermitExp { get; set; }
        public string? WasteHaulerPermit { get; set; }
        public string? EPACompliance { get; set; }
        public DateTime? EPAComplianceExp { get; set; }
        public string? Insurance { get; set; }
        public DateTime? InsuranceExp { get; set; }
        public Guid? ActivationLink { get; set; }
        public bool? HasLogin { get; set; }
        public string? ManagerFirstName { get; set; }
        public string? ManagerLastName { get; set; }
        public string? ManagerPhoneNumber { get; set; }
    }

    public class PersonDbRow
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }

    public class ProviderServiceDbRow
    {
        public long Id { get; set; }
        public long ProviderId { get; set; }
        public long ProductId { get; set; }
    }

    public class DriverDbRow
    {
        public long Id { get; set; }
        public Guid PersonId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? License { get; set; }
        public DateTime? LicenseExp { get; set; }
        public string? BackgroundCheck { get; set; }
        public DateTime? BackgroundCheckExp { get; set; }
        public string? ProfilePhoto { get; set; }
        public Guid? ActivationLink { get; set; }
        public int Status { get; set; }
        public bool TwoFactor { get; set; }
    }

    public class ProviderDriverDbRow
    {
        public long Id { get; set; }
        public long ProviderId { get; set; }
        public long DriverId { get; set; }
        public bool IsDefault { get; set; }
    }

    public class VehicleDbRow
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public long ProviderId { get; set; }
        public string? VehicleRegistration { get; set; }
        public DateTime? VehicleRegistrationExp { get; set; }
        public string? VehicleInsurance { get; set; }
        public DateTime? VehicleInsuranceExp { get; set; }
        public string? InspectionReport { get; set; }
        public DateTime? InspectionReportExp { get; set; }
        public string? Picture { get; set; }
        public int Capacity { get; set; }
        public double Weight { get; set; }
        public string? MeasurementCertificate { get; set; }
        public string? PeriodicVehicleInspections { get; set; }
        public DateTime? PeriodicVehicleInspectionsExp { get; set; }
        public int? CompartmentSize { get; set; }
        public string? LicenseNumber { get; set; }
    }

    public class ProviderVehicleDbRow
    {
        public long Id { get; set; }
        public long ProviderId { get; set; }
        public long VehicleId { get; set; }
    }

    public class VehicleCompartmentDbRow
    {
        public long Id { get; set; }
        public long VehicleId { get; set; }
        public int Capacity { get; set; }
    }

    public class VehicleServiceDbRow
    {
        public long Id { get; set; }
        public long VehicleId { get; set; }
        public int Capacity { get; set; }
        public int VehicleServiceStatus { get; set; }
    }
}
