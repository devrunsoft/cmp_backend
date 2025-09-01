using System;
using CMPNatural.Core.Enums;

namespace CMPNatural.Core.Entities
{
	public partial class Driver
	{
		public Driver()
		{
		}
        public long Id { get; set; }
        public Guid PersonId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string License { get; set; } = null!;
        public DateTime LicenseExp { get; set; }
        public string BackgroundCheck { get; set; } = null!;
        public DateTime BackgroundCheckExp { get; set; }
        public string? ProfilePhoto { get; set; }
        public long ProviderId { get; set; }
        public DriverStatus Status { get; set; }
        public bool TwoFactor { get; set; }
        public bool IsDefault { get; set; }

        public Person Person { get; set; }
    }

    public partial class DriverResponse
    {
        public DriverResponse()
        {
        }
        public long Id { get; set; }
        public Guid PersonId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string License { get; set; } = null!;
        public DateTime LicenseExp { get; set; }
        public string BackgroundCheck { get; set; } = null!;
        public DateTime BackgroundCheckExp { get; set; }
        public string? ProfilePhoto { get; set; }
        public long ProviderId { get; set; }
        public DriverStatus Status { get; set; }
        public bool TwoFactor { get; set; }
        public bool IsDefault { get; set; }

        public string FullName { get => $"{FirstName} {LastName}"; }
    }
}

