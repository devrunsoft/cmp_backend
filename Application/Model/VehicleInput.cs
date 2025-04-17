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
        public IFormFile VehicleRegistration { get; set; } = null!;
        public DateTime VehicleRegistrationExp { get; set; }
        public IFormFile VehicleInsurance { get; set; } = null!;
        public DateTime VehicleInsuranceExp { get; set; }
        public IFormFile InspectionReport { get; set; } = null!;
        public DateTime InspectionReportExp { get; set; }
        public IFormFile Picture { get; set; }
        public int Capacity { get; set; }
        public double Weight { get; set; }
        public IFormFile MeasurementCertificate { get; set; } = null!;
        public IFormFile PeriodicVehicleInspections { get; set; } = null!;
        public DateTime PeriodicVehicleInspectionsExp { get; set; }

        public int CapacityId { get; set; }

        public List<int> VehicleCompartments { get; set; } = new List<int>();
        public List<VehicleServiceStatus> VehicleService { get; set; } = new List<VehicleServiceStatus>();
    }
}

