using System;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Collections.Generic;
using CMPNatural.Core.Entities;

namespace CMPNatural.Application.Commands.Admin.Invoice
{
	public class AdminCheckLocationProviderCommand : IRequest<CommandResponse<List<Provider>>>
    {
		public AdminCheckLocationProviderCommand()
		{
		}
        public long LocationComanyId { get; set; }
        public long ProductId { get; set; }
    }
}

