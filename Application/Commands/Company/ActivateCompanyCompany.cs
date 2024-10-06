using System;
using CMPNatural.Application.Responses;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Commands.Company
{
    public class ActivateCompanyCompany : IRequest<CommandResponse<CompanyResponse>>
    {
        public Guid activationLink { get; set; }
    }
}

