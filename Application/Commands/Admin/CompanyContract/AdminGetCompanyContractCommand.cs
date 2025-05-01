using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
	public class AdminGetCompanyContractCommand : IRequest<CommandResponse<CompanyContract>>
    {
		public AdminGetCompanyContractCommand()
		{
		}
		public long Id { get; set; }
	}
}

