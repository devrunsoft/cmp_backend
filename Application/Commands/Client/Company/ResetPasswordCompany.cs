using System;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Commands.Company
{
	public class ResetPasswordCompanyCommand : IRequest<CommandResponse<object>>
    {
        public long CompanyId { get; set; }
        public string Password { get; set; }
        public string RePassword { get; set; }
    }
}

