using System;
using CMPNatural.Application.Responses;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Commands.Company
{
	public class GetCompanyByEmailCommand : IRequest<CommandResponse<object>>
    {
        public string Email { get; set; }
    }
}

