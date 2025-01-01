using System;
using Newtonsoft.Json.Linq;

namespace CmpNatural.CrmManagment.Model
{
	public class CustomValueResponse
	{
        public string id { get; set; }
        public string name { get; set; }
        public string fieldKey { get; set; }
        public string value { get; set; }
        public string locationId { get; set; }
    }
}
