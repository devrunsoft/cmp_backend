using System;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Commands
{
    public class EditLocationCompanyCommand : LocationCompanyInput, IRequest<CommandResponse<object>>
    {
        public EditLocationCompanyCommand()
        {
        }
        public long Id { get; set; }
        public long CompanyId { get; set; }

    }
}

