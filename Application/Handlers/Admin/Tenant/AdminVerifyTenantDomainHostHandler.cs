using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Application.Services;
using CMPNatural.Core.Repositories;
using CMPNatural.Core.Services;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
    public class AdminVerifyTenantDomainHostHandler : IRequestHandler<AdminVerifyTenantDomainHostCommand, CommandResponse<bool>>
    {
        private readonly ITenantDomainRepository _tenantDomainRepository;
        private readonly IHostVerificationService _hostVerificationService;
        private readonly CloudflareDnsService _cloudflareDnsService;

        public AdminVerifyTenantDomainHostHandler(
            ITenantDomainRepository tenantDomainRepository,
            IHostVerificationService hostVerificationService,
            CloudflareDnsService cloudflareDnsService)
        {
            _tenantDomainRepository = tenantDomainRepository;
            _hostVerificationService = hostVerificationService;
            _cloudflareDnsService = cloudflareDnsService;
        }

        public async Task<CommandResponse<bool>> Handle(AdminVerifyTenantDomainHostCommand request, CancellationToken cancellationToken)
        {
            var tenantDomain = await _tenantDomainRepository.GetByIdAsync(request.TenantDomainId);
            if (tenantDomain == null || string.IsNullOrWhiteSpace(tenantDomain.Host))
            {
                return new NoAcess<bool> { Data = false, Message = "Tenant domain host not found." };
            }

            var dnsExists = await _cloudflareDnsService.DnsRecordExistsAsync(tenantDomain.SubDomain ?? tenantDomain.Host);
            //var isValid = dnsExists && await _hostVerificationService.VerifyHostAsync(tenantDomain.Host, cancellationToken);
            var isValid = dnsExists;
            tenantDomain.Verified = isValid;
            tenantDomain.IsVerified = isValid;
            await _tenantDomainRepository.UpdateAsync(tenantDomain);

            return new Success<bool>() { Data = isValid };
        }
    }
}
