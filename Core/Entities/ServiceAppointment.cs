using System;
namespace CMPNatural.Core.Entities
{
	public partial class ServiceAppointment : BaseServiceAppointment
    {
		public int FrequencyType { get; set; }

		public DateTime StartDate { get; set; }
	}
}

