using System;
namespace CMPNatural.Core.Entities
{
	public partial class Capacity
	{
		public Capacity()
		{
		}
		public long Id { get; set; }
		public string Name { get; set; }
		public int Qty { get; set; }
		public int ServiceType { get; set; }
		public bool Enable { get; set; }
        public virtual ICollection<LocationCompany> LocationCompany { get; set; }
    }
}