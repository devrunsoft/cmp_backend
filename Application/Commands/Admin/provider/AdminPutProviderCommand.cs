using System;
using CMPNatural.Application.Model;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
	public class AdminPutProviderCommand : ProviderInput , IRequest<CommandResponse<Provider>>
    {
		public long Id { get; set; }
        public string BaseVirtualPath { get; set; }
    }
}

