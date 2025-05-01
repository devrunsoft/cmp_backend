using System;
using CMPNatural.Core.Enums;

namespace CMPNatural.Core.Entities
{
	public partial class Payment
	{
		public Payment()
		{
		}
		public long Id { get; set; }
		public string CheckoutSessionId { get; set; }
		public long InvoiceId { get; set; }
        public long CompanyId { get; set; }
		public double Amount { get; set; }
        public string Content { get; set; }
        public PaymentHistoryStatus Status { get; set; }
		public DateTime CreateAt { get; set; }

	}
}

