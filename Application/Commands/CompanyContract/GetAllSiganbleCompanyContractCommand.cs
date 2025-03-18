using System;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Collections.Generic;
using CMPNatural.Core.Entities;

namespace CMPNatural.Application
{
    public class GetAllSiganbleCompanyContractCommand : IRequest<CommandResponse<List<CompanyContract>>>
    {
        public GetAllSiganbleCompanyContractCommand()
        {
        }
        public long CompanyId { get; set; }
    }
}

