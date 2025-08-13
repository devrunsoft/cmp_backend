using System;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using ScoutDirect.Core.Caching;
using ScoutDirect.infrastructure.Repository;

namespace CMPNatural.infrastructure.Repository
{
    public class AdminRepository : Repository<AdminEntity, long>, IAdminRepository
    {
        public AdminRepository(ScoutDBContext context, Func<CacheTech, ICacheService> cacheService) : base(context, cacheService) { }

        public Task<AdminEntity> GetCachedByIdAsync(long id)
        => GetOrCreateAsync(Key("id", id),
        () => _dbContext.Set<AdminEntity>().FirstOrDefaultAsync(x => x.Id == id));
    }
}

