using System;
using CMPNatural.Core.Enums;

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

	}
}

