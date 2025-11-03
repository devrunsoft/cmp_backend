using System;
using CMPNatural.Core.Enums;

namespace CMPNatural.Core.Entities
{
    public partial class ServiceAppointmentLocation
    {
        public long Id { get; set; }

        public long ServiceAppointmentId { get; set; }

        public long LocationCompanyId { get; set; }

        public long? InvoiceId { get; set; }

        public int Qty { get; set; } = 1;

        public int? FactQty { get; set; }

        public OilQualityEnum? OilQuality { get; set; }

        public ServiceStatus Status { get; set; }

        public DateTime? StartedAt { get; set; }

        public DateTime? FinishDate { get; set; }

        public string? Comment { get; set; } = string.Empty;

        public virtual BaseServiceAppointment ServiceAppointment { get; set; }
        public virtual LocationCompany LocationCompany { get; set; }

        public virtual Manifest Manifest { get; set; }

        public virtual Invoice Invoice { get; set; }

    }

}