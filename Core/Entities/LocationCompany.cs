using System;
namespace CMPNatural.Core.Entities
{
    public partial class LocationCompany
    {
        public LocationCompany()
        {
        }
        public long Id { get; set; }
        public long CompanyId { get; set; }
        public long OperationalAddressId { get; set; }
        public string Name { get; set; }
        public double Lat { get; set; }
        public double Long { get; set; }
        public string Capacity { get; set; }
        public string Comment { get; set; }
        public string? PrimaryFirstName { get; set; }
        public string? PrimaryLastName { get; set; }
        public string? PrimaryPhonNumber { get; set; }
        public int Type { get; set; }

    }
}

