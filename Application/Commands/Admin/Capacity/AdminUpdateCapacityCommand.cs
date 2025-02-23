using System;
using CMPNatural.Application.Model;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
	public class AdminUpdateCapacityCommand : CapacityInput, IRequest<CommandResponse<Capacity>>
    {
		public AdminUpdateCapacityCommand()
		{
		}
		public long Id { get; set; }
	}
}