using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Entities;
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

        public AdminVerifyTenantDomainHostHandler(ITenantDomainRepository tenantDomainRepository, IHostVerificationService hostVerificationService)
        {
            _tenantDomainRepository = tenantDomainRepository;
            _hostVerificationService = hostVerificationService;
        }

        public async Task<CommandResponse<bool>> Handle(AdminVerifyTenantDomainHostCommand request, CancellationToken cancellationToken)
        {
            var tenantDomain = await _tenantDomainRepository.GetByIdAsync(request.TenantDomainId);
            var isValid = tenantDomain != null && await _hostVerificationService.VerifyHostAsync(tenantDomain.Host, cancellationToken);
            tenantDomain.Verified = isValid;
            await _tenantDomainRepository.UpdateAsync(tenantDomain);

            return new Success<bool>() { Data = isValid };
        }
    }
}
