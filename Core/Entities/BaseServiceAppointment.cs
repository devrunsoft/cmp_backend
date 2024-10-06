using System;
namespace CMPNatural.Core.Entities
{
	public partial class BaseServiceAppointment
	{
        public long Id { get; set; }

        public int ServiceTypeId { get; set; }

        public long CompanyId { get; set; }

        //public long LocationCompanyId { get; set; }

        public long OperationalAddressId { get; set; }

    }
}

