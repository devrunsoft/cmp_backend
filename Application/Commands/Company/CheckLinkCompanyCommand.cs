using System;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Commands.Company
{
    public class CheckLinkCompanyCommand : IRequest<CommandResponse<object>>
    {
        public Guid forgotPasswordLink { get; set; }
        public long CompanyId { get; set; }
    }
}

