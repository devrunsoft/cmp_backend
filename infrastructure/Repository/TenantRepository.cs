using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using ScoutDirect.Core.Caching;
using ScoutDirect.infrastructure.Repository;

namespace CMPNatural.infrastructure.Repository
{
    public class TenantRepository : Repository<Tenant, long>, ITenantRepository
    {
        public TenantRepository(ScoutDBContext context, Func<CacheTech, ICacheService> cacheService)
            : base(context, cacheService) { }

        public Task<Tenant?> GetActiveByHostAsync(string host)
        {
            var normalizedHost = host.Trim().ToLowerInvariant();

            return GetOrCreateAsync(
                Key("host", normalizedHost),
                () => _dbContext.Set<Tenant>()
                    .FirstOrDefaultAsync(x =>
                        x.IsDelete == null &&
                        x.IsActive &&
                        x.Host != null &&
                        x.Host.ToLower() == normalizedHost));
        }

        public Task<List<TenantAccess>> GetActiveAccessListAsync(long tenantId)
        {
            return GetOrCreateAsync(
                Key("access", tenantId),
                () => _dbContext.Set<TenantAccess>()
                    .Where(x =>
                        x.IsDelete == null &&
                        x.IsActive &&
                        x.TenantId == tenantId)
                    .ToListAsync());
        }
    }
}
