using System;
using CMPNatural.Core.Enums;

namespace CMPNatural.Application.Model
{
	public class CapacityInput
	{
		public CapacityInput()
		{
		}
        public string Name { get; set; }
        public int Qty { get; set; }
        public ServiceType ServiceType { get; set; }
        public bool Enable { get; set; }
        public int Order { get; set; }
    }
}

