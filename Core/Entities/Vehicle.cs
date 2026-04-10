using System;
namespace CMPNatural.Core.Entities
{
	public partial class Vehicle
	{
		public Vehicle()
		{

		}
		public long Id { get; set; }
        public string Name { get; set; }
        public long ProviderId { get; set; }
        public string? VehicleRegistration { get; set; } 
        public DateTime? VehicleRegistrationExp { get; set; }
        public string? VehicleInsurance { get; set; }
        public DateTime? VehicleInsuranceExp { get; set; }
        public string? InspectionReport  { get; set; } = null!;
        public DateTime? InspectionReportExp { get; set; }
        public string? Picture { get; set; }
        public int Capacity { get; set; }
        public double Weight { get; set; }
        public string? MeasurementCertificate { get; set; } 
        public string? PeriodicVehicleInspections { get; set; }
        public DateTime? PeriodicVehicleInspectionsExp { get; set; }

        public int? CompartmentSize { get; set; }
        public string? LicenseNumber { get; set; }

        public virtual ICollection<VehicleCompartment> VehicleCompartment { get; set; } = new List<VehicleCompartment>();
        public virtual ICollection<VehicleService> VehicleService { get; set; } = new List<VehicleService>();
        public virtual ICollection<ProviderVehicle> ProviderVehicle { get; set; } = new List<ProviderVehicle>();
    }
}
