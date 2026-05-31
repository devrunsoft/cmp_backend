using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
    public class AdminVerifyTenantDomainHostCommand : IRequest<CommandResponse<bool>>
    {
        public long TenantDomainId { get; set; }
    }
}
