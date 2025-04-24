using System;
using CMPNatural.Application.Responses.Client;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Commands.Client.Representation
{
	public class ClientMenuRepresentationCommand : IRequest<CommandResponse<ClientRepresentationResponse>>
    {
		public long CompanyId { get; set; }
	}
}

