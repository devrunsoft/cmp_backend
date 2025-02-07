using System;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Enums;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Commands.Admin
{

    public class AdminLoginCommand : IRequest<CommandResponse<AdminEntity>>
    {
        public string Email { get; set; }
        public string Password { get; set; }

    }
}

