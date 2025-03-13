using System;
using CMPNatural.Application.Responses.ProviderServiceAssignment;
using CMPNatural.Core.Base;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;
using ScoutDirect.Core.Base;

namespace CMPNatural.Application
{
	public class AdminGetAllProviderServiceAssignmentCommand : PagedQueryRequest, IRequest<CommandResponse<PagesQueryResponse<BaseServiceAppointmentResponse>>>
    {
		public AdminGetAllProviderServiceAssignmentCommand()
		{
		}

		public long? providerId { get; set; }
	}
}

