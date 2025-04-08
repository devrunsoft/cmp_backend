using System;
using CMPNatural.Core.Enums;

namespace CMPNatural.Application.Model
{
	public class ServiceAreaInput
	{
		public ServiceAreaInput()
		{
		}
        public string City { get; set; }
        public string State { get; set; }
        public string Address { get; set; }
        public ServiceAreaTypeEnum ServiceAreaType { get; set; }
        public long ProviderId { get; set; }

        public double? Lat { get; set; }
        public double? Lng { get; set; }
        public double? Radius { get; set; }

        public string? GeoJson { get; set; }
    }
}

