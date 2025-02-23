using System;
using CMPNatural.Application.Model;
using CMPNatural.Core.Base;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
	public class AdminAddCapacityCommand : CapacityInput , IRequest<CommandResponse<Capacity>>
    {
		public AdminAddCapacityCommand()
		{
		}
		public long Id { get; set; }
	}
}

