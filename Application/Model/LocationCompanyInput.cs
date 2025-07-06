using System;
using System.Collections.Generic;
using CMPNatural.Application.Model;
using CMPNatural.Core.Enums;

namespace CMPNatural.Application.Commands
{
    public class LocationCompanyInput
    {
        public LocationCompanyInput(){ }

        public string Name { get; set; }
        public string Address { get; set; }
        public double Lat { get; set; }
        public double Long { get; set; }
        public string Capacity { get; set; }
        public long CapacityId { get; set; }
        public string Comment { get; set; }
        public string PrimaryFirstName { get; set; }
        public string PrimaryLastName { get; set; }
        public string PrimaryPhonNumber { get; set; }
        public long OperationalAddressId { get; set; }
        public LocationType Type { get; set; }
    }
}

