using System;
namespace CMPNatural.Core.Entities
{
	public partial class Driver
	{
		public Driver()
		{
		}
        public long Id { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string License { get; set; } = null!;
        public DateTime LicenseExp { get; set; }
        public string BackgroundCheck { get; set; } = null!;
        public DateTime BackgroundCheckExp { get; set; }
        public string? ProfilePhoto { get; set; }
        public long ProviderId { get; set; }
    }
}

