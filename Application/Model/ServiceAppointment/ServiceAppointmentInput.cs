using System;
using CMPNatural.Core.Enums;

namespace CMPNatural.Application.Model.ServiceAppointment
{
	public class ServiceAppointmentInput
	{

        public ServiceType ServiceTypeId { get; set; }

        public long OperationalAddressId { get; set; }

        //public long LocationCompanyId { get; set; }

        public int FrequencyType { get; set; }

        public DateTime StartDate { get; set; }
    }
}

