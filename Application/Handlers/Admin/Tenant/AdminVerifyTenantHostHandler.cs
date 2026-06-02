using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Application.Services;
using CMPNatural.Core.Repositories;
using CMPNatural.Core.Services;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
    public class AdminVerifyTenantHostHandler : IRequestHandler<AdminVerifyTenantHostCommand, CommandResponse<bool>>
    {
        private readonly ITenantRepository _tenantRepository;
        private readonly IHostVerificationService _hostVerificationService;
        private readonly CloudflareDnsService _cloudflareDnsService;

        public AdminVerifyTenantHostHandler(
            ITenantRepository tenantRepository,
            IHostVerificationService hostVerificationService,
            CloudflareDnsService cloudflareDnsService)
        {
            _tenantRepository = tenantRepository;
            _hostVerificationService = hostVerificationService;
            _cloudflareDnsService = cloudflareDnsService;
        }

        public async Task<CommandResponse<bool>> Handle(AdminVerifyTenantHostCommand request, CancellationToken cancellationToken)
        {
            var tenant = await _tenantRepository.GetByIdAsync(request.TenantId);
            if (tenant == null || string.IsNullOrWhiteSpace(tenant.Host))
            {
                return new NoAcess<bool> { Data = false, Message = "Tenant host not found." };
            }

            var dnsExists = await _cloudflareDnsService.DnsRecordExistsAsync(tenant.SubDomain ?? tenant.Host);
            var serverDnsCheck= await _hostVerificationService.VerifyHostAsync(tenant.Host, cancellationToken);
            var isValid = dnsExists && serverDnsCheck;
            tenant.Verified = isValid;
            await _tenantRepository.UpdateAsync(tenant);


            if (!dnsExists)
            {
                return new Success<bool>
                {
                    Data = false,
                    Message = "DNS record was not found. Please verify your DNS configuration and try again."
                };
            }

            if (!isValid)
            {
                return new Success<bool>
                {
                    Data = false,
                    Message = "DNS record was found, but the domain is not yet pointing to the required server. Please wait for DNS propagation and try again."
                };
            }



            return new Success<bool>() { Data = isValid, Message = "Domain verification completed successfully." };
        }
    }
}
