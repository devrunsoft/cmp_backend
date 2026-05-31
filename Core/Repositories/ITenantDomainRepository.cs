using CMPNatural.Core.Entities;
using ScoutDirect.Core.Repositories.Base;

namespace CMPNatural.Core.Repositories
{
    public interface ITenantDomainRepository : IRepository<TenantDomain, long>
    {
        Task<TenantDomain?> GetByHostAsync(string host);
        Task<List<TenantDomain>> GetByTenantIdAsync(long tenantId);
    }
}
