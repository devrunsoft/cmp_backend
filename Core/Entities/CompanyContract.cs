using System;
using System.ComponentModel.DataAnnotations.Schema;
using CMPNatural.Core.Enums;

namespace CMPNatural.Core.Entities
{
	public partial class CompanyContract
	{
		public long Id { get; set; }
        public string Content { get; set; } = "";
		public long ContractId { get; set; }
        public long CompanyId { get; set; }
        public string InvoiceId { get; set; }
        public string? Sign { get; set; }
        public string? AdminSign { get; set; }
        public DateTime? ClientSignDate { get; set; }
        public CompanyContractStatus Status { get; set; }
        public long OperationalAddressId { get; set; }
        public string ContractNumber { get; set; }

        public Company Company { get; set; }

        [NotMapped]
        public string Number {
			get {
				return $"C{CreatedAt.Year}-{this.CompanyId}-{this.Id}";
			}
		} 
        public DateTime CreatedAt { get; set; }

    }
}

