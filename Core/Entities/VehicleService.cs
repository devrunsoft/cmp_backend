using System;
namespace CMPNatural.Core.Entities
{
	public partial class VehicleService
	{
		public VehicleService()
		{
		}
		public long Id { get; set; }
		public long VehicleId { get; set; }
        public long ProductId { get; set; }
    }
}

