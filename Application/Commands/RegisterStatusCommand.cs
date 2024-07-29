using System;
using CMPNatural.Application.Model;
using CMPNatural.Core.Enums;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Commands
{
    public class RegisterStatusCommand : IRequest<CommandResponse<RegisterType>>
    {
        public long CompanyId { get; set; }
    }
}

