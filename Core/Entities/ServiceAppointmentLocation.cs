using System;
using CMPNatural.Core.Enums;

namespace CMPNatural.Core.Entities
{
    public partial class ServiceAppointmentLocation
    {
        public long Id { get; set; }

        public long ServiceAppointmentId { get; set; }

        public long LocationCompanyId { get; set; }

        public int Qty { get; set; } = 1;

        public int? FactQty { get; set; }

        public OilQualityEnum? OilQuality { get; set; }

        public ServiceStatus Status { get; set; }

        public DateTime? FinishDate { get; set; }

        public virtual BaseServiceAppointment ServiceAppointment { get; set; }
        public virtual LocationCompany LocationCompany { get; set; }
    }
}