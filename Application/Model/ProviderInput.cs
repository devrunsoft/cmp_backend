using System;
using CMPNatural.Core.Enums;

namespace CMPNatural.Application.Model
{
	public class ProviderInput
	{
        public string Name { get; set; } = "";
        public double Rating { get; set; }
        public ProviderStatus Status { get; set; }

        public double Lat { get; set; }
        public double Long { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string County { get; set; }
    }
}

