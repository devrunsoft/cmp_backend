using System;
using CMPNatural.Core.Enums;

namespace CMPNatural.Application.Responses.Service
{
	public class ServicePriceResponse
	{
		public ServicePriceResponse()
		{
		}
        public string _id { get; set; }
        public string name { get; set; }
        public ServicePriceType type { get; set; }
        public string currency { get; set; }
        public string amount { get; set; }
        public string product { get; set; }
        public bool deleted { get; set; }

    }
}
