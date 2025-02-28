using System;
using System.Collections.Generic;
using CMPNatural.Core.Enums;

namespace CMPNatural.Application.Model.ServiceAppointment
{
	public class ServiceAppointmentInput
	{

        public ServiceType ServiceTypeId { get; set; } = ServiceType.Cooking_Oil_Collection;

        public long ProductPriceId { get; set; }

        public long ProductId { get; set; }

        public long OperationalAddressId { get; set; }

        public List<long> LocationCompanyIds { get; set; }

        public string FrequencyType { get; set; } = null;

        public DateTime? StartDate { get; set; } = null;

        public ServiceKind ServiceKind { get; set; } = ServiceKind.Custom;

        public int qty { get; set; } = 1;
    }
}

