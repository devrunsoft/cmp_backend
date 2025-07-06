using System;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using infrastructure.Data;
using ScoutDirect.Core.Caching;
using ScoutDirect.infrastructure.Repository;

namespace CMPNatural.infrastructure.Repository
{
    public class RequestTerminateRepository : Repository<RequestTerminate, long>, IRequestTerminateRepository
    {
        public RequestTerminateRepository(ScoutDBContext context, Func<CacheTech, ICacheService> cacheService) : base(context, cacheService) { }
    }
}

