using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
    public class AdminRemoveTenantDomainHostFromCloudflareCommand : IRequest<CommandResponse<bool>>
    {
        public long TenantDomainId { get; set; }
    }
}
