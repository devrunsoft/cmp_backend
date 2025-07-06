using System;
namespace CMPNatural.Core.Entities
{
	public partial class InvoiceSource
	{
		public InvoiceSource()
		{
		}

		public long Id { get; set; }
		public string InvoiceId { get; set; }
		public long CompanyId { get; set; }
		public long BillingInformationId { get; set; }
        public long OperationalAddressId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}