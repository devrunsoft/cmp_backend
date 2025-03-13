using System;
using CMPNatural.Application.Model;
using CMPNatural.Core.Base;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
	public class AdminDeleteCapacityCommand : IRequest<CommandResponse<Capacity>>
    {
		public AdminDeleteCapacityCommand()
		{
		}
		public long Id { get; set; }
	}
}

