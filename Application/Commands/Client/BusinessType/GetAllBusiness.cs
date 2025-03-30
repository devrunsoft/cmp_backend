using System;
using System.Collections.Generic;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Commands
{
	public class GetAllBusinessTypeCommand : IRequest<CommandResponse<List<BusinessType>>>
	{
		public GetAllBusinessTypeCommand()
		{
		}
	}
}

