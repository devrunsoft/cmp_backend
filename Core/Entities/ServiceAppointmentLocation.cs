using System;
namespace CMPNatural.Core.Entities
{
    public partial class ServiceAppointmentLocation
    {
        public long Id { get; set; }

        public long ServiceAppointmentId { get; set; }

        public long LocationCompanyId { get; set; }

        public virtual BaseServiceAppointment ServiceAppointment { get; set; }
        public virtual LocationCompany LocationCompany { get; set; }

    }
}