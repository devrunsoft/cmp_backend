using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Application.Services;
using CMPNatural.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Configuration;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
    public class AdminAddTenantHostToCloudflareHandler : IRequestHandler<AdminAddTenantHostToCloudflareCommand, CommandResponse<bool>>
    {
        private readonly ITenantRepository _tenantRepository;
        private readonly CloudflareDnsService _cloudflareDnsService;
        private readonly IConfiguration _configuration;

        public AdminAddTenantHostToCloudflareHandler(
            ITenantRepository tenantRepository,
            CloudflareDnsService cloudflareDnsService,
            IConfiguration configuration)
        {
            _tenantRepository = tenantRepository;
            _cloudflareDnsService = cloudflareDnsService;
            _configuration = configuration;
        }

        public async Task<CommandResponse<bool>> Handle(AdminAddTenantHostToCloudflareCommand request, CancellationToken cancellationToken)
        {
            var tenant = await _tenantRepository.GetByIdAsync(request.TenantId);
            if (tenant == null || (string.IsNullOrWhiteSpace(tenant.SubDomain) && string.IsNullOrWhiteSpace(tenant.Host)))
            {
                return new NoAcess<bool> { Data = false, Message = "Tenant host not found." };
            }

            await _cloudflareDnsService.CreateTenantWildcardDnsAsync(tenant.SubDomain ?? tenant.Host!, _configuration["Cloudflare:BackendServerIp"]);
            tenant.Verified = true;
            await _tenantRepository.UpdateAsync(tenant);

            return new Success<bool> { Data = true };
        }
    }
}
