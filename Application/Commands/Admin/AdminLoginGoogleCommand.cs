using System;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Commands.Admin
{
	public class AdminLoginGoogleCommand : IRequest<CommandResponse<AdminEntity>>
    {
        public string Credential { get; set; }
    }
}

