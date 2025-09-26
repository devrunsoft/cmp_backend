using System;
using CMPNatural.Core.Enums;

namespace CMPNatural.Core.Entities
{
	public partial class Route
	{
		public long Id { get; set; }
		public DateTime Date { get; set; }
		public string Name { get; set; }
        public RouteStatus Status { get; set; }
        public long DriverId { get; set; }
        public long? VehicleId { get; set; }
        public long ProviderId { get; set; }
        public DateTime CreateAt { get; set; }
		public virtual ICollection<RouteServiceAppointmentLocation> Items { get; set; } = new List<RouteServiceAppointmentLocation>();

    }
}

