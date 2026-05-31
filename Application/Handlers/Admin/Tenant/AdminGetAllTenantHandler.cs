using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Base;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
    public class AdminGetAllTenantHandler : IRequestHandler<AdminGetAllTenantCommand, CommandResponse<PagesQueryResponse<Tenant>>>
    {
        private readonly ITenantRepository _tenantRepository;

        public AdminGetAllTenantHandler(ITenantRepository tenantRepository)
        {
            _tenantRepository = tenantRepository;
        }

        public async Task<CommandResponse<PagesQueryResponse<Tenant>>> Handle(AdminGetAllTenantCommand request, CancellationToken cancellationToken)
        {
            var result = await _tenantRepository.GetBasePagedAsync(
                request,
                x =>
                    (!request.IsActive.HasValue || x.IsActive == request.IsActive.Value) &&
                    (!request.AllowMainAdminAccess.HasValue || x.AllowMainAdminAccess == request.AllowMainAdminAccess.Value) &&
                    (!request.AdminCanViewAllRecords.HasValue || x.AdminCanViewAllRecords == request.AdminCanViewAllRecords.Value) &&
                    (!request.WantsDispatchManagement.HasValue || x.WantsDispatchManagement == request.WantsDispatchManagement.Value),
                query => query.Include(x => x.Domains));

            return new Success<PagesQueryResponse<Tenant>>() { Data = result };
        }
    }
}
