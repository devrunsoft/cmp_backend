using System;
namespace CMPNatural.Core.Entities
{
	public partial class CompanyContract
	{
		public CompanyContract()
		{
		}
		public long Id { get; set; }
        public string Content { get; set; } = "";
		public long ContractId { get; set; }
        public long CompanyId { get; set; }
        public string InvoiceId { get; set; }
        public string Sign { get; set; }
        public int Status { get; set; }
		public Company Company { get; set; }
        //public Invoice Invoice { get; set; }

        public string UniqueId {
			get {
				return $"{this.ContractId}-{this.CompanyId}";
			}
		} 
        public DateTime CreatedAt { get; set; }

    }
}

