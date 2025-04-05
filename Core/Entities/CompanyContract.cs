using System;
using CMPNatural.Core.Enums;

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
        public string? Sign { get; set; }
        public string? AdminSign { get; set; }
        public DateTime? ClientSignDate { get; set; }
        public int Status { get; set; }
        //public CancelEnum? CancelBy { get; set; }
        public Company Company { get; set; }
        //public Invoice Invoice { get; set; }

        public string ContractNumber {
			get {
				return $"{CreatedAt.Year}-{this.CompanyId}-{this.Id}";
			}
		} 
        public DateTime CreatedAt { get; set; }

    }
}

