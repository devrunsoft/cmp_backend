using System;
using CMPNatural.Core.Enums;

namespace CMPNatural.Application.Model.ServiceAppointment
{
	public class ServiceAppointmentEmergencyInput
	{
        public ServiceType ServiceTypeId { get; set; }

        //public long LocationCompanyId { get; set; }

        public long OperationalAddressId { get; set; }

        public int FrequencyType { get; set; }
    }
}

