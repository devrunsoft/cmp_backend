using System;
using System.Collections.Generic;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
	public class AdminGetAllMenuByAdminCommand : IRequest<CommandResponse<List<Menu>>>
    {
		public AdminGetAllMenuByAdminCommand()
		{
		}
		public long AdminId { get; set; }
	}
}

