// using System.Linq;
// using System.Threading;
// using System.Threading.Tasks;
// using CMPNatural.Application.Services;
// using CMPNatural.Core.Entities;
// using CMPNatural.Core.Repositories;
// using MediatR;
// using ScoutDirect.Application.Responses;
// using ScoutDirect.Core.Repositories.Base;

// namespace CMPNatural.Application
// {
//     public class AdminDeleteWhiteLabelRequestHandler : IRequestHandler<AdminDeleteWhiteLabelRequestCommand, CommandResponse<bool>>
//     {
//         private readonly IWhiteLabelRequestRepository _whiteLabelRequestRepository;
//         private readonly ITenantRepository _tenantRepository;
//         private readonly ITenantDomainRepository _tenantDomainRepository;
//         private readonly IAdminRepository _adminRepository;
//         private readonly IRepository<TenantAccess, long> _tenantAccessRepository;
//         private readonly CloudflareDnsService _cloudflareDnsService;

//         public AdminDeleteWhiteLabelRequestHandler(
//             IWhiteLabelRequestRepository whiteLabelRequestRepository,
//             ITenantRepository tenantRepository,
//             ITenantDomainRepository tenantDomainRepository,
//             IAdminRepository adminRepository,
//             IRepository<TenantAccess, long> tenantAccessRepository,
//             CloudflareDnsService cloudflareDnsService)
//         {
//             _whiteLabelRequestRepository = whiteLabelRequestRepository;
//             _tenantRepository = tenantRepository;
//             _tenantDomainRepository = tenantDomainRepository;
//             _adminRepository = adminRepository;
//             _tenantAccessRepository = tenantAccessRepository;
//             _cloudflareDnsService = cloudflareDnsService;
//         }

//         public async Task<CommandResponse<bool>> Handle(AdminDeleteWhiteLabelRequestCommand request, CancellationToken cancellationToken)
//         {
//             var whiteLabelRequest = await _whiteLabelRequestRepository.GetByIdAsync(request.WhiteLabelRequestId);
//             if (whiteLabelRequest == null)
//             {
//                 return new NoAcess<bool> { Data = false, Message = "White-label request not found." };
//             }

//             var tenant = (await _tenantRepository.GetAsync(x => x.WhiteLabelRequestId == request.WhiteLabelRequestId)).FirstOrDefault();
//             if (tenant != null)
//             {
//                 if (!string.IsNullOrWhiteSpace(tenant.SubDomain) || !string.IsNullOrWhiteSpace(tenant.Host))
//                 {
//                     await _cloudflareDnsService.DeleteDnsRecordIfExistsAsync(tenant.SubDomain ?? tenant.Host!);
//                 }

//                 var tenantDomains = await _tenantDomainRepository.GetByTenantIdAsync(tenant.Id);
//                 foreach (var tenantDomain in tenantDomains)
//                 {
//                     if (!string.IsNullOrWhiteSpace(tenantDomain.SubDomain) || !string.IsNullOrWhiteSpace(tenantDomain.Host))
//                     {
//                         await _cloudflareDnsService.DeleteDnsRecordIfExistsAsync(tenantDomain.SubDomain ?? tenantDomain.Host);
//                     }
//                 }

//                 var tenantAccesses = (await _tenantAccessRepository.GetAsync(
//                     x => x.TenantId == tenant.Id || x.AccessibleTenantId == tenant.Id)).ToList();
//                 if (tenantAccesses.Count > 0)
//                 {
//                     await _tenantAccessRepository.DeleteRangeAsync(tenantAccesses);
//                 }

//                 var tenantAdmins = (await _adminRepository.GetAsync(x => x.TenantId == tenant.Id)).ToList();
//                 if (tenantAdmins.Count > 0)
//                 {
//                     await _adminRepository.DeleteRangeAsync(tenantAdmins);
//                 }

//                 if (tenantDomains.Count > 0)
//                 {
//                     await _tenantDomainRepository.DeleteRangeAsync(tenantDomains);
//                 }

//                 await _tenantRepository.DeleteAsync(tenant);
//             }

//             await _whiteLabelRequestRepository.DeleteAsync(whiteLabelRequest);

//             return new Success<bool> { Data = true };
//         }
//     }
// }
