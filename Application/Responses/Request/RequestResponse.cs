using System;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMPNatural.Application
{
    public partial class RequestResponse
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
        public long? ProviderId { get; set; }
        public InvoiceStatus Status { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public string? Link { get; set; }
        public double Amount { get; set; }
        public DateTime? SendDate { get; set; }
        public DateTime CreatedAt { get; set; }

        public long OperationalAddressId { get; set; }

        public string Address { get; set; } = "";

        public string? Comment { get; set; }

        public long? ContractId { get; set; }

        public InvoiceType Type { get; set; }

        public string RequestNumber { get; set; }

        public long BillingInformationId { get; set; }

        public virtual Company Company { get; set; } = null!;

        public virtual Provider? Provider { get; set; }

        public virtual BillingInformation BillingInformation { get; set; } = null!;

        [NotMapped]
        public string ReqNumber
        {
            get
            {
                return $"R{this.CreatedAt.Year}-{this.CompanyId}/{this.OperationalAddressId}-{Id}";
            }
        }

        public virtual ICollection<BaseServiceAppointment> BaseServiceAppointment { get; set; } = new List<BaseServiceAppointment>();

        public virtual OperationalAddress? OperationalAddress { get; set; }
    }
}

