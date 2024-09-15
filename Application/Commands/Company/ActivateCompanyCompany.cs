using System;
using CMPNatural.Application.Responses;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Commands.Company
{
    public class ActivateCompanyCompany : IRequest<CommandResponse<object>>
    {
        public Guid activationLink { get; set; }
    }
}

