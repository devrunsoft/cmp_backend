using System;
namespace CMPNatural.Core.Entities
{
	public partial class Invoice
	{
		public long Id { get; set; }
		public long CompanyId { get; set; }
        //public string ProductCrmId { get; set; }
        //public string ProductPriceCrmId { get; set; }
        public string InvoiceCrmId { get; set; } = null!;
        public string? Status { get; set; }
        public string? Link { get; set; }
        public double Amount { get; set; }
        //public Guid InvoiceNumber { get; set; }
        public string InvoiceId { get; set; } = null!;

        public DateTime RegisterDate { get; set; }

        public Company Company { get; set; }

        public virtual ICollection<BaseServiceAppointment> BaseServiceAppointment { get; set; }
    }
}

