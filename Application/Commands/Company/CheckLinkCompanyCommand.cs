using System;
using CMPNatural.Application.Responses;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Commands.Company
{
    public class CheckLinkCompanyCommand : IRequest<CommandResponse<CompanyResponse>>
    {
        public Guid forgotPasswordLink { get; set; }
        public long CompanyId { get; set; }
    }
}

