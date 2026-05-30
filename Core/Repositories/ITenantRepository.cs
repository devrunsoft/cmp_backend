using CMPNatural.Core.Entities;
using ScoutDirect.Core.Repositories.Base;

namespace CMPNatural.Core.Repositories
{
    public interface ITenantRepository : IRepository<Tenant, long>
    {
        Task<Tenant?> GetActiveByHostAsync(string host);
        Task<List<TenantAccess>> GetActiveAccessListAsync(long tenantId);
    }
}
