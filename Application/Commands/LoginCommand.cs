using System;
using MediatR;
using ScoutDirect.Application.Responses;
using ScoutDirect.Application.Responses.Base;

namespace CMPNatural.Application.Commands
{
    public class LoginCompanyCommand : IRequest<CommandResponse<object>>
    {
        public string BusinessEmail { get; set; } = null!;
        public string Password { get; set; } = null!;

    }
}

