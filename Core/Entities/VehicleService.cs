using System;
using CMPNatural.Core.Enums;

namespace CMPNatural.Core.Entities
{
	public partial class VehicleService
	{
		public VehicleService()
		{
		}
		public long Id { get; set; }
		public long VehicleId { get; set; }
        public VehicleServiceStatus VehicleServiceStatus { get; set; }
    }
}

