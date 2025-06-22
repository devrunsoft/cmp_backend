using System;
using System.ComponentModel.DataAnnotations.Schema;
using CMPNatural.Core.Enums;

namespace CMPNatural.Core.Entities
{
	public partial class Invoice
	{
		public long Id { get; set; }
		public long CompanyId { get; set; }
        public long? ProviderId { get; set; }
        public InvoiceStatus Status { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public string? Link { get; set; }
        public double Amount { get; set; }
        public string InvoiceId { get; set; } = null!;
        //public DateTime RegisterDate { get; set; }
        public DateTime? SendDate { get; set; }
        public DateTime CreatedAt { get; set; }

        public long OperationalAddressId { get; set; }
        public string Address { get; set; } = "";

        public string? Comment { get; set; }

        public virtual Company Company { get; set; } = null!;

        public virtual Provider? Provider { get; set; }

        public long? ContractId { get; set; }

        public string InvoiceNumber { get; set; }

        public string RequestNumber { get; set; }

        [NotMapped]
        public string Number
        {
            get
            {
                return this.ContractId == null ? "---" : $"I{this.CreatedAt.Year}-{this.CompanyId}/{this.OperationalAddressId}-{(this.ContractId == null ? 0 : this.ContractId)}-{Id}";
            }
        }

        [NotMapped]
        public string ReqNumber
        {
            get
            {
                return $"R{this.CreatedAt.Year}-{this.CompanyId}-{Id}";
            }
        }



        public virtual ICollection<InvoiceProduct> InvoiceProduct { get; set; } = new List<InvoiceProduct>();

        public virtual ICollection<BaseServiceAppointment> BaseServiceAppointment { get; set; } = new List<BaseServiceAppointment>();
    }
}

