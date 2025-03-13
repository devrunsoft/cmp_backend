using System;
namespace CMPNatural.Core.Entities
{
	public partial class Provider
	{
        public long Id { get; set; }
        public string Name { get; set; } = "";
        public double Rating { get; set; }
        public int Status { get; set; }
        public double Lat { get; set; }
        public double Long { get; set; }
        public double AreaLocation { get; set; }
        public string City { get; set; } = "";
        public string Address { get; set; } = "";
        public string County { get; set; } = "";

        public string? BusinessLicense { get; set; }
        public DateTime? BusinessLicenseExp { get; set; }

        public string? HealthDepartmentPermit { get; set; }
        public DateTime? HealthDepartmentPermitExp { get; set; }

        public string? WasteHaulerPermit { get; set; }

        public string? EPACompliance { get; set; }
        public DateTime? EPAComplianceExp { get; set; }

        public string? Insurance { get; set; }
        public DateTime? InsuranceExp { get; set; }

        public virtual ICollection<ProviderService> ProviderService { get; set; } = new List<ProviderService>();


    }
}

