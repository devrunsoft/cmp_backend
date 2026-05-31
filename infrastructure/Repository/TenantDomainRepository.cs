using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using ScoutDirect.Core.Caching;
using ScoutDirect.infrastructure.Repository;

namespace CMPNatural.infrastructure.Repository
{
    public class TenantDomainRepository : Repository<TenantDomain, long>, ITenantDomainRepository
    {
        public TenantDomainRepository(ScoutDBContext context, Func<CacheTech, ICacheService> cacheService)
            : base(context, cacheService) { }

        public Task<TenantDomain?> GetByHostAsync(string host)
        {
            var normalizedHost = host.Trim().ToLowerInvariant();

            return GetOrCreateAsync(
                Key("host", normalizedHost),
                () => BaseQuery()
                    .Include(x => x.Tenant)
                    .FirstOrDefaultAsync(x => x.Host.ToLower() == normalizedHost));
        }

        public Task<List<TenantDomain>> GetByTenantIdAsync(long tenantId)
        {
            return GetOrCreateAsync(
                Key("tenant", tenantId),
                () => BaseQuery()
                    .Where(x => x.TenantId == tenantId)
                    .OrderByDescending(x => x.IsPrimary)
                    .ThenBy(x => x.Id)
                    .ToListAsync());
        }
    }
}
