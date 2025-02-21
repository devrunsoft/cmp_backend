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
        public IFormFile License { get; set; }
        public DateTime LicenseExp { get; set; }
        public IFormFile BackgroundCheck { get; set; }
        public DateTime BackgroundCheckExp { get; set; }
        public IFormFile ProfilePhoto { get; set; }

    }
}

