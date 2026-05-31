using System.Threading;
using System.Threading.Tasks;
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

        public AdminVerifyTenantHostHandler(ITenantRepository tenantRepository, IHostVerificationService hostVerificationService)
        {
            _tenantRepository = tenantRepository;
            _hostVerificationService = hostVerificationService;
        }

        public async Task<CommandResponse<bool>> Handle(AdminVerifyTenantHostCommand request, CancellationToken cancellationToken)
        {
            var tenant = await _tenantRepository.GetByIdAsync(request.TenantId);
            var isValid = tenant != null && await _hostVerificationService.VerifyHostAsync(tenant.Host, cancellationToken);
            tenant.Verified = isValid;
            await _tenantRepository.UpdateAsync(tenant);

            return new Success<bool>() { Data = isValid };
        }
    }
}
