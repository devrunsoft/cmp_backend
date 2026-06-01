using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Application.Services;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
    public class AdminRemoveTenantDomainHostFromCloudflareHandler : IRequestHandler<AdminRemoveTenantDomainHostFromCloudflareCommand, CommandResponse<bool>>
    {
        private readonly ITenantDomainRepository _tenantDomainRepository;
        private readonly CloudflareDnsService _cloudflareDnsService;

        public AdminRemoveTenantDomainHostFromCloudflareHandler(
            ITenantDomainRepository tenantDomainRepository,
            CloudflareDnsService cloudflareDnsService)
        {
            _tenantDomainRepository = tenantDomainRepository;
            _cloudflareDnsService = cloudflareDnsService;
        }

        public async Task<CommandResponse<bool>> Handle(AdminRemoveTenantDomainHostFromCloudflareCommand request, CancellationToken cancellationToken)
        {
            var tenantDomain = await _tenantDomainRepository.GetByIdAsync(request.TenantDomainId);
            if (tenantDomain == null || (string.IsNullOrWhiteSpace(tenantDomain.SubDomain) && string.IsNullOrWhiteSpace(tenantDomain.Host)))
            {
                return new NoAcess<bool> { Data = false, Message = "Tenant domain host not found." };
            }

            await _cloudflareDnsService.DeleteDnsRecordIfExistsAsync(tenantDomain.SubDomain ?? tenantDomain.Host);
            tenantDomain.Verified = false;
            tenantDomain.IsVerified = false;
            await _tenantDomainRepository.UpdateAsync(tenantDomain);

            return new Success<bool> { Data = true };
        }
    }
}
