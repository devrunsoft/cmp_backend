using System;
using CMPNatural.Core.Entities;
using System.Collections.Generic;
using CMPNatural.Application.Model.ServiceAppointment;

namespace CMPNatural.Application.Model
{
	public class InvoiceInput
	{
        public double Amount { get; set; }
        public string Address { get; set; } = "";
        public bool ForceToPay { get; set; } = false;
        public List<ServiceAppointmentInput> ServiceAppointment { get; set; } = new List<ServiceAppointmentInput>();
    }
}

