using System;
namespace CMPNatural.Core.Entities
{
	public partial class VehicleCompartment
	{
		public VehicleCompartment()
		{
		}
		public long Id { get; set; }
		public long VehicleId { get; set; }
        //gallons
        public int Capacity { get; set; }
    }
}

