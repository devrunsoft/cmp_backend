using System;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Collections.Generic;
using CMPNatural.Application.Model;
using CMPNatural.Core.Entities;

namespace CMPNatural.Application
{
	public class AddDriverCommand : DriverInput, IRequest<CommandResponse<DriverResponse>>
    {
		public AddDriverCommand()
		{
		}
        public long ProviderId { get; set; }
        public string BaseVirtualPath { get; set; }
    }
}

