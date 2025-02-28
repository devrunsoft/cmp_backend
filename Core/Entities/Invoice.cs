using System;
namespace CMPNatural.Core.Entities
{
	public partial class Invoice
	{
		public long Id { get; set; }
		public long CompanyId { get; set; }
        public long? ProviderId { get; set; }
        //public string ProductCrmId { get; set; }
        //public string ProductPriceCrmId { get; set; }
        public string InvoiceCrmId { get; set; } = null!;
        public int Status { get; set; }
        public string? Link { get; set; }
        public double Amount { get; set; }
        //public Guid InvoiceNumber { get; set; }
        public string InvoiceId { get; set; } = null!;
        public DateTime RegisterDate { get; set; }
        public DateTime? SendDate { get; set; }

        public long OperationalAddressId { get; set; }
        public string Address { get; set; } = "";

        public Company Company { get; set; } = null!;

        public Provider? Provider { get; set; }

        public virtual ICollection<InvoiceProduct> InvoiceProduct { get; set; } = new List<InvoiceProduct>();

        public virtual ICollection<BaseServiceAppointment> BaseServiceAppointment { get; set; } = new List<BaseServiceAppointment>();
    }
}

