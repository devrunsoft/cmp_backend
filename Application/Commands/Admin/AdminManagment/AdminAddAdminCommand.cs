using System;
using CMPNatural.Application.Model;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Handlers.Admin.AdminManagment
{
	public class AdminAddAdminCommand : AdminInput, IRequest<CommandResponse<AdminEntity>>
    {
		public AdminAddAdminCommand()
		{
		}
	}
}

