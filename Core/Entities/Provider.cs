using System;
using System.Collections.Generic;
using System.Xml.Linq;
using CMPNatural.Core.Enums;

namespace CMPNatural.Core.Entities
{
	public partial class Provider
	{
        public long Id { get; set; }
        public Guid? PersonId { get; set; }
        public string Name { get; set; } = "";
        public double? Rating { get; set; } = 0;
        public ProviderStatus Status { get; set; }
        public double? Lat { get; set; } = 0;
        public double? Long { get; set; } = 0;
        public double? AreaLocation { get; set; } = 0;
        public string? City { get; set; } = "";
        public string? Address { get; set; } = "";
        public string? County { get; set; } = "";
        public string Email { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public string? Password { get; set; } = "";
        public ProviderRegistrationStatus RegistrationStatus { get; set; } = ProviderRegistrationStatus.Complete;

        public string? BusinessLicense { get; set; }
        public DateTime? BusinessLicenseExp { get; set; }

        public string? HealthDepartmentPermit { get; set; }
        public DateTime? HealthDepartmentPermitExp { get; set; }

        public string? WasteHaulerPermit { get; set; }

        public string? EPACompliance { get; set; }
        public DateTime? EPAComplianceExp { get; set; }

        public string? Insurance { get; set; }
        public DateTime? InsuranceExp { get; set; }

        public Guid? ActivationLink { get; set; } = null;
        public bool? HasLogin { get; set; }

        public string? ManagerFirstName { get; set; }
        public string? ManagerLastName { get; set; }
        public string? ManagerPhoneNumber { get; set; }
       

        public virtual ICollection<ProviderService> ProviderService { get; set; } = new List<ProviderService>();
        public virtual ICollection<ServiceArea> ServiceArea { get; set; } = new List<ServiceArea>();


    }
}

