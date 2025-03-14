using MediatR;
using ScoutDirect.Application.Responses;
using CMPNatural.Core.Entities;

namespace CMPNatural.Application
{
    public class SignCompanyContractCommand : IRequest<CommandResponse<CompanyContract>>
    {
        public SignCompanyContractCommand()
        {
        }
        public long CompanyId { get; set; }
        public long CompanyContractId { get; set; }
        public string Sign { get; set; }
    }
}

