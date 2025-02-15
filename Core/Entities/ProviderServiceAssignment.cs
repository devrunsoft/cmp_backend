using System;
namespace CMPNatural.Core.Entities
{
	public partial class ProviderServiceAssignment
	{
        public ProviderServiceAssignment()
        {
        }
        public long Id { get; set; }
		public long ProviderId { get; set; }
        public long InvoiceId { get; set; }
        public long CompanyId { get; set; }
        public int Status { get; set; }
        public DateTime AssignTime { get; set; }
        public Invoice Invoice { get; set; }
        public Company Company { get; set; }
    }
}
