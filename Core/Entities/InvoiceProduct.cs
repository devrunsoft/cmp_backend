using System;
namespace CMPNatural.Core.Entities
{
	public partial class InvoiceProduct
	{
		public long Id { get; set; }
		public long ProductPriceId { get; set; }
        public long InvoiceId { get; set; }

		public virtual ProductPrice ProductPrice { get; set; }
        public virtual Invoice Invoice { get; set; }
    }
}

