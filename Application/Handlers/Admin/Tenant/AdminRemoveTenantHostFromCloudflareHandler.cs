using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Application.Services;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
    public class AdminRemoveTenantHostFromCloudflareHandler : IRequestHandler<AdminRemoveTenantHostFromCloudflareCommand, CommandResponse<bool>>
    {
        private readonly ITenantRepository _tenantRepository;
        private readonly CloudflareDnsService _cloudflareDnsService;

        public AdminRemoveTenantHostFromCloudflareHandler(
            ITenantRepository tenantRepository,
            CloudflareDnsService cloudflareDnsService)
        {
            _tenantRepository = tenantRepository;
            _cloudflareDnsService = cloudflareDnsService;
        }

        public async Task<CommandResponse<bool>> Handle(AdminRemoveTenantHostFromCloudflareCommand request, CancellationToken cancellationToken)
        {
            var tenant = await _tenantRepository.GetByIdAsync(request.TenantId);
            if (tenant == null || (string.IsNullOrWhiteSpace(tenant.SubDomain) && string.IsNullOrWhiteSpace(tenant.Host)))
            {
                return new NoAcess<bool> { Data = false, Message = "Tenant host not found." };
            }

            await _cloudflareDnsService.DeleteDnsRecordIfExistsAsync(tenant.SubDomain ?? tenant.Host!);
            tenant.Verified = false;
            await _tenantRepository.UpdateAsync(tenant);

            return new Success<bool> { Data = true };
        }
    }
}
