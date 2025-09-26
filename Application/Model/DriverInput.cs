using System;
using Microsoft.AspNetCore.Http;

namespace CMPNatural.Application.Model
{
	public class DriverInput
	{
		public DriverInput()
		{
		}
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Email { get; set; }
        public bool IsDefault { get; set; }
        public string License { get; set; }
        public DateTime LicenseExp { get; set; }
        public string BackgroundCheck { get; set; }
        public DateTime BackgroundCheckExp { get; set; }
        public string ProfilePhoto { get; set; }

    }
}

