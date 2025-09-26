using System;
using System.Collections.Generic;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Enums;
using Microsoft.AspNetCore.Http;

namespace CMPNatural.Application.Model
{
	public class VehicleInput
	{
		public VehicleInput()
		{
		}

        public string Name { get; set; }
        public string VehicleRegistration { get; set; } = null!;
        public DateTime VehicleRegistrationExp { get; set; }
        public string VehicleInsurance { get; set; } = null!;
        public DateTime VehicleInsuranceExp { get; set; }
        public string InspectionReport { get; set; } = null!;
        public DateTime InspectionReportExp { get; set; }
        public string Picture { get; set; }
        public int Capacity { get; set; }
        public double Weight { get; set; }
        public string MeasurementCertificate { get; set; } = null!;
        public string PeriodicVehicleInspections { get; set; } = null!;
        public DateTime PeriodicVehicleInspectionsExp { get; set; }

        public List<int> VehicleCompartments { get; set; } = new List<int>();
        public List<VehicleServiceInput> VehicleService { get; set; } = new List<VehicleServiceInput>();
    }
    public partial class VehicleServiceInput
    {
        public int Capacity { get; set; }
        public VehicleServiceStatus VehicleServiceStatus { get; set; }
    }
}

