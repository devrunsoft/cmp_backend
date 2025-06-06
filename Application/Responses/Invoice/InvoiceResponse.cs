using System;
using System.Collections.Generic;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Enums;

namespace CMPNatural.Application
{
	public class InvoiceResponse
	{
        public long Id { get; set; }
        public long CompanyId { get; set; }
        public long? ProviderId { get; set; }
        //public string ProductCrmId { get; set; }
        //public string ProductPriceCrmId { get; set; }
        //public string InvoiceCrmId { get; set; } = null!;
        public InvoiceStatus Status { get; set; }
        public string? Link { get; set; }
        public double Amount { get; set; }
        //public Guid InvoiceNumber { get; set; }
        public string InvoiceId { get; set; } = null!;
        public long OperationalAddressId { get; set; }
        public string Address { get; set; } = "";
        public string InvoiceNumber { get; set; }
        public string RequestNumber { get; set; }
        public virtual Provider Provider { get; set; } = null;
        public virtual Company Company { get; set; } = null;
        public virtual BillingInformation BillingInformation { get; set; } = null;
        //public string InvoiceNumber { get; set; }
        public int? ContractId { get; set; }

        public virtual ICollection<InvoiceProduct> InvoiceProduct { get; set; } = new List<InvoiceProduct>();

        public virtual ICollection<BaseServiceAppointment> BaseServiceAppointment { get; set; } = new List<BaseServiceAppointment>();


        public string? Comment { get; set; }

        public DateTime CreatedAt { get; set; }

        public string? InvoiceStatus {
            get {

                return ((Core.Enums.InvoiceStatus)Status).GetDescription();
            }
        }

        public bool CanAssign
        {
            get
            {
                return
                    //Status == (int)Core.Enums.InvoiceStatus.paid &&
                    ProviderId==null;

            }
        }

    }
}

