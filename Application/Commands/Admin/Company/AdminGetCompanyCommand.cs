using System;
using CMPNatural.Application.Responses;
using CMPNatural.Core.Base;
using MediatR;
using ScoutDirect.Application.Responses;
using ScoutDirect.Core.Base;

namespace CMPNatural.Application.Commands.Admin.Company
{
    public class AdminGetCompanyCommand : IRequest<CommandResponse<CompanyResponse>>
    {
        public long Id { get; set; }
    }
}

