using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Enums;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
    public class AdminReviewWhiteLabelRequestHandler : IRequestHandler<AdminReviewWhiteLabelRequestCommand, CommandResponse<WhiteLabelRequest>>
    {
        private readonly IWhiteLabelRequestRepository _repository;
        private readonly ITenantRepository _tenantRepository;
        private readonly ITenantDomainRepository _tenantDomainRepository;

        public AdminReviewWhiteLabelRequestHandler(IWhiteLabelRequestRepository repository, ITenantRepository tenantRepository, ITenantDomainRepository tenantDomainRepository)
        {
            _repository = repository;
            _tenantRepository = tenantRepository;
            _tenantDomainRepository = tenantDomainRepository;
        }

        public async Task<CommandResponse<WhiteLabelRequest>> Handle(AdminReviewWhiteLabelRequestCommand request, CancellationToken cancellationToken)
        {
            if (request.Status != WhiteLabelStatus.Accepted && request.Status != WhiteLabelStatus.Rejected)
            {
                return new NoAcess<WhiteLabelRequest>() { Message = "Only Accepted or Rejected status is allowed." };
            }

            var entity = await _repository.GetByIdAsync(request.WhiteLabelRequestId);
            if (entity == null)
            {
                return new NoAcess<WhiteLabelRequest>() { Message = "White-label request not found." };
            }

            entity.Status = request.Status;
            entity.AdminReviewNote = request.AdminReviewNote??"";
            entity.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(entity);

            if (entity.Status == WhiteLabelStatus.Accepted)
            {
                var domain = (entity.WantsCustomDomain ? entity.CustomDomain : entity.SubDomain);
                var tenent = new Tenant()
                {
                    Host = "api." + domain,
                    AdminCanViewAllRecords = false,
                    AllowMainAdminAccess = false,
                    TenantAccesses = entity.TenantId == null ?
                    new List<TenantAccess>() :
                    new List<TenantAccess>()
                    {
                        new TenantAccess()
                        {
                            AccessibleTenantId = entity.TenantId.Value
                        }
                    }
                };
                tenent = await _tenantRepository.AddAsync(tenent);

                List<TenantDomain> domains = new List<TenantDomain>();
                if (entity.WantsClientPortal)
                {
                    domains.Add(new TenantDomain()
                    {
                        TenantId = tenent.Id,
                        Host = "client." + domain,
                        PortalType = PortalType.Client
                    });
                }

                if (!entity.WantsDispatchManagement)
                {
                    domains.Add(new TenantDomain()
                    {
                        TenantId = tenent.Id,
                        Host = "admin." + domain,
                        PortalType = PortalType.Admin
                    });
                }

                domains.Add(new TenantDomain()
                {
                    TenantId = tenent.Id,
                    Host = "provider." + domain,
                    PortalType = PortalType.Provider
                });

                await _tenantDomainRepository.AddRangeAsync(domains);
            }

            return new Success<WhiteLabelRequest>() { Data = entity };
        }
    }
}
