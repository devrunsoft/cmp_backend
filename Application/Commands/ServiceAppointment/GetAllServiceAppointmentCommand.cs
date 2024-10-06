using System;
using System.Collections.Generic;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
	public class GetAllServiceAppointmentCommand : IRequest<CommandResponse<List<ServiceAppointment>>>
    {
		public long CompanyId { get; set; }
	}
}

