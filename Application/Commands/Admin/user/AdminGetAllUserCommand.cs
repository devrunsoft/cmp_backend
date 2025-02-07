using System;
using System.Collections.Generic;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
	public class AdminGetAllUserCommand : IRequest<CommandResponse<List<Company>>>
    {
		public AdminGetAllUserCommand()
		{
		}
	}
}

