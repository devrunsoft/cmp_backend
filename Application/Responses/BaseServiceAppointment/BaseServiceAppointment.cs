using System;
using System.Collections.Generic;
using CMPNatural.Application;

namespace CMPNatural.Core.Entities
{
    public partial class BaseServiceAppointmentResponse
    {
        public long Id { get; set; }

        public long ServiceTypeId { get; set; }

        public string? ServicePriceCrmId { get; set; }

        public string? ServiceCrmId { get; set; }

        public long ProductPriceId { get; set; }

        public long ProductId { get; set; }

        public long? ProviderId { get; set; }

        public long CompanyId { get; set; }

        public int Status { get; set; }

        public long OperationalAddressId { get; set; }

        public long InvoiceId { get; set; }

        public bool IsEmegency { get; set; }

        public int Qty { get; set; } = 1;

        public DateTime StartDate { get; set; }

        public double Amount { get; set; }

        public virtual InvoiceResponse Invoice { get; set; }

        public virtual Product Product { get; set; }

        public virtual ProductPrice ProductPrice { get; set; }

        public virtual ICollection<ServiceAppointmentLocation> ServiceAppointmentLocations { get; set; } = new List<ServiceAppointmentLocation>();

    }
}

