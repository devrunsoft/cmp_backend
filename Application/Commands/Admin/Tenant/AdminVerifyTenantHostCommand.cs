using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
    public class AdminVerifyTenantHostCommand : IRequest<CommandResponse<bool>>
    {
        public long TenantId { get; set; }
    }
}
