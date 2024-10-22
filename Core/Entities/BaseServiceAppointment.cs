using System;
namespace CMPNatural.Core.Entities
{
	public partial class BaseServiceAppointment
	{
        public long Id { get; set; }

        public long ServiceTypeId { get; set; }

        public string? ServicePriceCrmId { get; set; }

        public string? ServiceCrmId { get; set; }

        public long CompanyId { get; set; }

        public int Status { get; set; }

        public long OperationalAddressId { get; set; }
        
        public long InvoiceId { get; set; }

        public Invoice Invoice { get; set; }

    }
}

