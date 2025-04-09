using System;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
	public class ProviderGetDocumentCommand : IRequest<CommandResponse<Provider>>
    {
		public ProviderGetDocumentCommand()
		{
		}
	}
}

