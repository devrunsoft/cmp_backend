using System;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Commands
{
	public class LogoCommand : IRequest<CommandResponse<string>>
    {
	}
}

