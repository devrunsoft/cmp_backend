using System;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Collections.Generic;
using CMPNatural.Core.Entities;

namespace CMPNatural.Application
{
	public class GetCompanyContractCommand : IRequest<CommandResponse<CompanyContract>>
    {
		public GetCompanyContractCommand()
		{
		}
        public long CompanyId { get; set; }
        public long ContractId { get; set; }
    }
}

