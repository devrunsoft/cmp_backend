using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Commands.Company
{
    public class RequestCompanyEmailChangeCommand : IRequest<CommandResponse<string>>
    {
        public long CompanyId { get; set; }
        public string Email { get; set; } = string.Empty;
    }
}
