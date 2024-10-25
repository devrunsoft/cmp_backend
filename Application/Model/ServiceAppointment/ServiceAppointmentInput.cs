using System;
using CMPNatural.Core.Enums;

namespace CMPNatural.Application.Model.ServiceAppointment
{
	public class ServiceAppointmentInput
	{

        public ServiceType ServiceTypeId { get; set; } = ServiceType.Cooking_Oil_Collection;

        public string ServicePriceId { get; set; }

        public string ServiceId { get; set; } = null!;

        public long OperationalAddressId { get; set; }

        public string FrequencyType { get; set; } = null;

        public DateTime StartDate { get; set; }

        public ServiceKind ServiceKind { get; set; } = ServiceKind.Custom;

    }
}

