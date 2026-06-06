using System;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Commands
{
	public class AdminCancelServiceCommand : IRequest<CommandResponse<BaseServiceAppointment>>
    {
		public long ServiceAppointmentId { get; set; }
	}
}

