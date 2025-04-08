using System;
using CMPNatural.Core.Enums;

namespace CMPNatural.Core.Entities
{
	public partial class ServiceArea
	{
		public ServiceArea()
		{
		}
		public long Id { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public ServiceAreaTypeEnum ServiceAreaType { get; set; } // "circle" or "polygon"
        public long ProviderId { get; set; }
        public string Address { get; set; }
        public DateTime CreateAt { get; set; }
        public bool Active { get; set; }

        //For "circle"
        public double? Lat { get; set; }
        public double? Lng { get; set; }
        public double? Radius { get; set; }

        // For "polygon"
        public string? GeoJson { get; set; }

        public virtual Provider Provider { get; set; }

    }
}

