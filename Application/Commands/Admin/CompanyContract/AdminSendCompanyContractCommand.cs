using MediatR;
using ScoutDirect.Application.Responses;
using CMPNatural.Core.Entities;

namespace CMPNatural.Application
{
    public class AdminSendCompanyContractCommand : IRequest<CommandResponse<CompanyContract>>
    {
        public AdminSendCompanyContractCommand()
        {
        }
        public long Id { get; set; }
    }
}

