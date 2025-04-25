using System;
using CMPNatural.Core.Enums;
using Microsoft.AspNetCore.Http;

namespace CMPNatural.Application.Model
{
	public class UpdateCompanyInput
	{

        public string CompanyName { get; set; }
        public string PrimaryFirstName { get; set; }
        public string PrimaryLastName { get; set; }
        public string PrimaryPhonNumber { get; set; }
        public string Position { get; set; }
        public string SecondaryFirstName { get; set; }
        public string SecondaryLastName { get; set; }
        public string SecondaryPhoneNumber { get; set; }
        public string BillingAddress { get; set; } = "";
        public string ZIPCode { get; set; } = "";
        public string City { get; set; } = "";
        public string State { get; set; } = "";
        public IFormFile ProfilePicture { get; set; } = null;

    }
    public class AddCompanyInput : UpdateCompanyInput
    {

        public string BusinessEmail { get; set; }

    }
}

