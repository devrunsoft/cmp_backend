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
		public DateTime CreatedAt { get; set; }
    }
}