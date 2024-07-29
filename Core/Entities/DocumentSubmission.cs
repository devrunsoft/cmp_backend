using System;
namespace CMPNatural.Core.Entities
{
	public partial class DocumentSubmission
	{
		public DocumentSubmission()
		{
		}
		public long Id { get; set; }
		public string BusinessLicense { get; set; }
        public string HealthDepartmentCertificate { get; set; }
		public long CompanyId { get; set; }

	}
}

