using System;
using System.Collections.Generic;

namespace CMPNatural.Application.Model
{
	public class OperationalAddressInput
	{
        //public long Id { get; set; }
        //public long CompanyId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string CrossStreet { get; set; }
        public string County { get; set; }
        public string LocationPhone { get; set; }
        public long BusinessId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public double Lat { get; set; }
        public double Long { get; set; }
        public List<LocationDateTimeInput> LocationDateTimeInputs { get; set; } = new List<LocationDateTimeInput>();

    }
}

