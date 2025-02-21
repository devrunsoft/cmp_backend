using System;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Collections.Generic;

namespace CMPNatural.Application
{
	public class GetAllDriverCommand : IRequest<CommandResponse<List<Driver>>>
    {
		public GetAllDriverCommand()
		{
		}
		public long ProviderId { get; set; }
	}
}

