using System;
using CMPNatural.Core.Entities;

namespace CMPNatural.Application
{
	public class InvoiceResponse
	{
		public InvoiceResponse()
		{
		}
        public long Id { get; set; }
        public long CompanyId { get; set; }
        public long? ProviderId { get; set; }
        //public string ProductCrmId { get; set; }
        //public string ProductPriceCrmId { get; set; }
        public string InvoiceCrmId { get; set; } = null!;
        public string? Status { get; set; }
        public string? Link { get; set; }
        public double Amount { get; set; }
        //public Guid InvoiceNumber { get; set; }
        public string InvoiceId { get; set; } = null!;

        public Provider? Provider { get; set; }

        public DateTime RegisterDate { get; set; }

        public string? InvoiceStatus {
            get {
                return ProviderId==null ? Status : "assigned";
            }
        }

        public bool CanAssign
        {
            get
            {
                return Status == "paid" && ProviderId==null;

            }
        }

    }
}

