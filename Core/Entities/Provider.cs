using System;
namespace CMPNatural.Core.Entities
{
	public partial class Provider
	{
        public long Id { get; set; }
        public string Name { get; set; } = "";
        public double Rating { get; set; }
        public int Status { get; set; }
        public double Lat { get; set; }
        public double Long { get; set; }
        public string City { get; set; } = "";
        public string Address { get; set; } = "";
        public string County { get; set; } = "";
    }
}

