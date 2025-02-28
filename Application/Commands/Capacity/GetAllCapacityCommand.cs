using System;
using System.Collections.Generic;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Enums;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Commands
{
	public class GetAllCapacityCommand : IRequest<CommandResponse<List<Capacity>>>
    {
		public GetAllCapacityCommand()
		{
		}
		public int ServiceType { get; set; }
	}
}

