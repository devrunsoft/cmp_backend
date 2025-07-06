using System;
using System.ComponentModel.DataAnnotations.Schema;
using CMPNatural.Core.Enums;

namespace CMPNatural.Core.Entities
{
	public partial class RequestTerminate
	{
        public long Id { get; set; }

        public string InvoiceNumber { get; set; }

        public long OperationalAddressId { get; set; }

        public long CompanyId { get; set; }

        public long ContractId { get; set; }

        public string Message { get; set; }

        public RequestTerminateEnum Status { get; set; }

        public RequestTerminateProcessEnum? RequestTerminateStatus { get; set; }

        public DateTime CreatedAt { get; set; }

        public string RequestTerminateNumber { get; set; }

        [NotMapped]
        public string Number
        {
            get
            {
                return $"T{this.CreatedAt.Year}-{this.CompanyId}/{this.OperationalAddressId}-{(this.ContractId)}-{Id}";
            }
        }

        public virtual ICollection<Invoice> Invoice { get; set; } = new List<Invoice>();
        public virtual Company Company { get; set; }
    }
}

