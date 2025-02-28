using System;
using System.Collections.Generic;
using CMPNatural.Core.Entities;

namespace CMPNatural.Application
{
	public class InvoiceResponse
	{
		public InvoiceResponse()
		{
		}
        public long Id { get; set; }
        public long CompanyId { get; set; }
        public long? ProviderId { get; set; }
        //public string ProductCrmId { get; set; }
        //public string ProductPriceCrmId { get; set; }
        public string InvoiceCrmId { get; set; } = null!;
        public int Status { get; set; }
        public string? Link { get; set; }
        public double Amount { get; set; }
        //public Guid InvoiceNumber { get; set; }
        public string InvoiceId { get; set; } = null!;
        public long OperationalAddressId { get; set; }
        public string Address { get; set; } = "";
        public Provider Provider { get; set; } = null;
        public Company Company { get; set; } = null;

        public virtual ICollection<InvoiceProduct> InvoiceProduct { get; set; } = new List<InvoiceProduct>();

        public virtual ICollection<BaseServiceAppointment> BaseServiceAppointment { get; set; } = new List<BaseServiceAppointment>();


        public DateTime RegisterDate { get; set; }

        public string? InvoiceStatus {
            get {

                return ProviderId==null ? ((Core.Enums.InvoiceStatus)Status).GetDescription() : "assigned";
            }
        }

        public bool CanAssign
        {
            get
            {
                return Status == (int)Core.Enums.InvoiceStatus.paid && ProviderId==null;

            }
        }

    }
}

