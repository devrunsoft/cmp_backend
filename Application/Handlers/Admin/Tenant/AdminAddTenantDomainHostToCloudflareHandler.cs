using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Application.Services;
using CMPNatural.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Configuration;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
    public class AdminAddTenantDomainHostToCloudflareHandler : IRequestHandler<AdminAddTenantDomainHostToCloudflareCommand, CommandResponse<bool>>
    {
        private readonly ITenantDomainRepository _tenantDomainRepository;
        private readonly CloudflareDnsService _cloudflareDnsService;
        private readonly IConfiguration _configuration;

        public AdminAddTenantDomainHostToCloudflareHandler(
            ITenantDomainRepository tenantDomainRepository,
            CloudflareDnsService cloudflareDnsService,
            IConfiguration configuration)
        {
            _tenantDomainRepository = tenantDomainRepository;
            _cloudflareDnsService = cloudflareDnsService;
            _configuration = configuration;
        }

        public async Task<CommandResponse<bool>> Handle(AdminAddTenantDomainHostToCloudflareCommand request, CancellationToken cancellationToken)
        {
            var tenantDomain = await _tenantDomainRepository.GetByIdAsync(request.TenantDomainId);
            if (tenantDomain == null || (string.IsNullOrWhiteSpace(tenantDomain.SubDomain) && string.IsNullOrWhiteSpace(tenantDomain.Host)))
            {
                return new NoAcess<bool> { Data = false, Message = "Tenant domain host not found." };
            }

            await _cloudflareDnsService.CreateTenantWildcardDnsAsync(tenantDomain.SubDomain ?? tenantDomain.Host, _configuration["Cloudflare:ServerIp"]);
            tenantDomain.Verified = true;
            tenantDomain.IsVerified = true;
            await _tenantDomainRepository.UpdateAsync(tenantDomain);

            return new Success<bool> { Data = true };
        }
    }
}
