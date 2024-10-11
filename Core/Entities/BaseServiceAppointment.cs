using System;
namespace CMPNatural.Core.Entities
{
	public partial class BaseServiceAppointment
	{
        public long Id { get; set; }

        public long ServiceTypeId { get; set; }

        public string? ServicePriceId { get; set; }


        public long CompanyId { get; set; }

        //public long LocationCompanyId { get; set; }

        public long OperationalAddressId { get; set; }

    }
}

