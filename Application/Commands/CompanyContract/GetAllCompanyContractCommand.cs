using System;
using System.Collections.Generic;
using CMPNatural.Application.Responses;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
	public class GetAllCompanyContractCommand : IRequest<CommandResponse<List<CompanyContract>>>
    {
		public GetAllCompanyContractCommand()
		{
		}
		public long CompanyId { get; set; }
	}
}

