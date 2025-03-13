using System;
namespace CMPNatural.Core.Entities
{
	public partial class AppInformation
	{
		public AppInformation()
		{
		}

		public long Id { get; set; }
		public string CompanyTitle { get; set; }
		public string CompanyIcon { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyPhoneNumber { get; set; }
        public string CompanyCeoLastName { get; set; }
        public string CompanyCeoFirstName { get; set; }
		public string Sign { get; set; }
	}
}
