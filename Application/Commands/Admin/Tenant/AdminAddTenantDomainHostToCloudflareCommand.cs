using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
    public class AdminAddTenantDomainHostToCloudflareCommand : IRequest<CommandResponse<bool>>
    {
        public long TenantDomainId { get; set; }
    }
}
