using System;
using CMPNatural.Core.Entities;

namespace CMPNatural.Application.Responses.ClientServiceAppointment
{
	public class ClientServiceAppointment
	{
		public ClientServiceAppointment()
		{
		}
        public BaseServiceAppointment? Draft { get; set; }
        public BaseServiceAppointment? Current { get; set; }
		public BaseServiceAppointment? Next { get; set; }
        public long ServiceId { get; set; }
        public bool CanTerminate { get; set; }
        public string InvoiceNumber { get; set; }
    }
}

