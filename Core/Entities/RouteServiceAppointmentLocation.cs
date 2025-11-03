using System;
namespace CMPNatural.Core.Entities
{
	public partial class RouteServiceAppointmentLocation
	{
		public long Id { get; set; }
		public long ServiceAppointmentLocationId { get; set; }
		public long RouteId { get; set; }
        public long ManifestId { get; set; }
        public string ManifestNumber { get; set; }

		public virtual ServiceAppointmentLocation ServiceAppointmentLocation { get; set; }

        public virtual Route Route { get; set; }
    }
}

