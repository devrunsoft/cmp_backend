using MediatR;
using ScoutDirect.Application.Responses;
using CMPNatural.Core.Entities;

namespace CMPNatural.Application
{
    public class AdminUpdateCompanyContractCommand : IRequest<CommandResponse<CompanyContract>>
    {
        public AdminUpdateCompanyContractCommand()
        {
        }
        public long CompanyContractId { get; set; }
        public long ContractId { get; set; }
    }
}

