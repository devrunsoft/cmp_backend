using System;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using infrastructure.Data;
using ScoutDirect.Core.Caching;
using ScoutDirect.infrastructure.Repository;

namespace CMPNatural.infrastructure.Repository
{
    public class DriverManifestRepository : Repository<DriverManifest, long>, IDriverManifestRepository
    {
        public DriverManifestRepository(ScoutDBContext context, Func<CacheTech, ICacheService> cacheService) : base(context, cacheService) { }
    }
}

