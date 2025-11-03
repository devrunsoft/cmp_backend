using System;
using CMPNatural.Core.Enums;

namespace CMPNatural.Core.Entities
{
	public partial class ServiceAppointmentLocationFile
	{
		public long Id { get; set; }
		public string Link { get; set; } = string.Empty;
		public long DriverId { get; set; }
        public long ServiceAppointmentLocationId { get; set; }
        public long RouteId { get; set; }
        public ServiceAppointmentLocationFileEnum Status { get; set; }
	}
}

