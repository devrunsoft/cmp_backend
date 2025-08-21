using System;
using System.Collections.Generic;
using CMPNatural.Application.Responses;
using CMPNatural.Core.Base;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Enums;
using MediatR;
using ScoutDirect.Application.Responses;
using ScoutDirect.Core.Base;

namespace CMPNatural.Application
{
	public class GetAllCompanyContractCommand : PagedQueryRequest, IRequest<CommandResponse<PagesQueryResponse<CompanyContract>>>
    {
		public GetAllCompanyContractCommand()
		{
		}
        public CompanyContractStatus? Status { get; set; }
        public long CompanyId { get; set; }
		public long OperationalAddressId { get; set; }
	}
}

