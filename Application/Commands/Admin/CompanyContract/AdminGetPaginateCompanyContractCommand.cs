using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;
using CMPNatural.Core.Enums;
using CMPNatural.Core.Base;
using ScoutDirect.Core.Base;

namespace CMPNatural.Application
{
	public class AdminGetPaginateCompanyContractCommand : PagedQueryRequest, IRequest<CommandResponse<PagesQueryResponse<CompanyContract>>>
    {
		public AdminGetPaginateCompanyContractCommand()
		{
		}
		public CompanyContractStatus? Status { get; set; }
		public long? CompanyId { get; set; }
	}
}

