using MediatR;
using ScoutDirect.Application.Responses;
using CMPNatural.Core.Entities;

namespace CMPNatural.Application
{
    public class AdminSignCompanyContractCommand : IRequest<CommandResponse<CompanyContract>>
    {
        public AdminSignCompanyContractCommand()
        {
        }
        public long CompanyId { get; set; }
        public long CompanyContractId { get; set; }
    }
}

