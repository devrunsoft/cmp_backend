using System;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Collections.Generic;

namespace CMPNatural.Application.Commands.Admin.ProviderServiceArea
{
	public class AdminGetAllProviderServiceAreaCommand : IRequest<CommandResponse<List<ServiceArea>>>
    {
		public AdminGetAllProviderServiceAreaCommand()
		{
		}
		public long ProviderId { get; set; }
	}
}

