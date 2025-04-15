using System.Collections.Generic;
using CMPNatural.Application.Model.ServiceAppointment;

namespace CMPNatural.Application.Model
{
    public class ProviderInvoiceInput
    {
        public double Amount { get; set; }
        public string Address { get; set; } = "";
        public List<ServiceAppointmentInput> ServiceAppointment { get; set; } = new List<ServiceAppointmentInput>();
    }
}

