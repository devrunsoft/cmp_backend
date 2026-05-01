using System;
using CMPNatural.Application.Responses;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Commands
{

    public class LoginGoogleCompanyCommand : IRequest<CommandResponse<CompanyResponse>>
    {
        public string Credential { get; set; }
    }
}

