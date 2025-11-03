using System;
using CMPNatural.Core.Enums;

namespace CMPNatural.Core.Entities
{
    public partial class BaseServiceAppointmentRequest
    {
        public long Id { get; set; }

        public long ServiceTypeId { get; set; }

        public string? ServicePriceCrmId { get; set; }

        public string? ServiceCrmId { get; set; }

        public long ProductPriceId { get; set; }

        public long ProductId { get; set; }

        public long? ProviderId { get; set; }

        public long CompanyId { get; set; }

        public ServiceStatus Status { get; set; }

        public CancelEnum? CancelBy { get; set; }

        public long OperationalAddressId { get; set; }

        public long RequestId { get; set; }

        public long? InvoiceId { get; set; }

        public bool IsEmegency { get; set; }

        public int Qty { get; set; } = 1;

        //public int? FactQty { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? ScaduleDate { get; set; }

        public double Amount { get; set; }

        public decimal? TotalAmount { get; set; }

        public string DayOfWeek { get; set; } = "";

        public int FromHour { get; set; } = 480; //8 AM

        public int ToHour { get; set; } = 1080; //6 PM

        public double Subsidy { get; set; }

        //public OilQualityEnum? OilQuality { get; set; }

        public virtual RequestEntity Request { get; set; }

        public virtual Invoice Invoice { get; set; }

        public virtual Product Product { get; set; }

        public virtual ProductPrice ProductPrice { get; set; }

        public virtual ICollection<ServiceAppointmentLocation> ServiceAppointmentLocations { get; set; } = new List<ServiceAppointmentLocation>();

    }
}

