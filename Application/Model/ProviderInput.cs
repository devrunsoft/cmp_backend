using System;
using CMPNatural.Core.Enums;
using Microsoft.AspNetCore.Http;

namespace CMPNatural.Application.Model
{
	public class ProviderInput
	{
        public string Name { get; set; } = "";
        public double Rating { get; set; }
        public ProviderStatus Status { get; set; }

        public double Lat { get; set; }
        public double Long { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string County { get; set; }

        public IFormFile BusinessLicense { get; set; }
        public DateTime? BusinessLicenseExp { get; set; }

        public IFormFile HealthDepartmentPermit { get; set; }
        public DateTime? HealthDepartmentPermitExp { get; set; }

        public IFormFile WasteHaulerPermit { get; set; }

        public IFormFile EPACompliance { get; set; }
        public DateTime? EPAComplianceExp { get; set; }

        public IFormFile Insurance { get; set; }
        public DateTime? InsuranceExp { get; set; }
    }
}

