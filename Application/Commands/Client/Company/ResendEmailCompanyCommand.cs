using System;
using CMPNatural.Application.Responses;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Commands.Company
{
	public class ResendEmailCompanyCommand : IRequest<CommandResponse<object>>
    {
        public long CompanyId { get; set; }
        public Guid ActivationLink { get; set; }
    }
}

