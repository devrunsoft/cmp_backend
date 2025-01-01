using System;
using System.Collections.Generic;

namespace CMPNatural.Application.Responses.Service
{
	public class ServiceResponse
	{
		public ServiceResponse()
		{
		}

        public string _id { get; set; }
        public string name { get; set; }
        public string productType { get; set; }
        public string description { get; set; }
        public string image { get; set; }
        public List<string> collectionIds { get; set; }
        public double minimum { get; set; } = 0;
    }
}
