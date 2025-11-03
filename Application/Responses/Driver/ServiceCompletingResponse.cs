using System;
using CMPNatural.Core.Entities;
using System.Collections.Generic;

namespace CMPNatural.Application.Responses.Driver
{
	public class ServiceCompletingResponse
	{
		public List<ServiceAppointmentLocation> Services { get; set; }
		public bool IsDone { get; set; }
	}
}

