using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
    public class AdminRemoveTenantHostFromCloudflareCommand : IRequest<CommandResponse<bool>>
    {
        public long TenantId { get; set; }
    }
}
