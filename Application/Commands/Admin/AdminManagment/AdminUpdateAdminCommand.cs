using System;
using CMPNatural.Application.Model;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
	public class AdminUpdateAdminCommand : AdminInput, IRequest<CommandResponse<AdminEntity>>
    {
		public AdminUpdateAdminCommand()
		{
		}
		public long Id { get; set; }
	}
}

