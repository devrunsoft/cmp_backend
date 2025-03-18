using System;
namespace CMPNatural.Core.Entities
{
	public partial class BillingInformation
    {
		public BillingInformation()
		{
		}
		public long Id { get; set; }
		public string? CardholderName { get; set; }
		public string? CardNumber { get; set; }
		public int Expiry { get; set; }
        public string? CVC { get; set; }
		public string Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? ZIPCode { get; set; }
        public bool IsPaypal { get; set; }
		public long CompanyId { get; set; }
		public virtual Company Company { get; set; }
	}
}

