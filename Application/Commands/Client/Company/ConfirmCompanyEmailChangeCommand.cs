using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Commands.Company
{
    public class ConfirmCompanyEmailChangeCommand : IRequest<CommandResponse<object>>
    {
        public long CompanyId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
    }
}
