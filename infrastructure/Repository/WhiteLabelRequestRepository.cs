using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using ScoutDirect.Core.Caching;
using ScoutDirect.infrastructure.Repository;

namespace CMPNatural.infrastructure.Repository
{
    public class WhiteLabelRequestRepository : Repository<WhiteLabelRequest, long>, IWhiteLabelRequestRepository
    {
        public WhiteLabelRequestRepository(ScoutDBContext context, Func<CacheTech, ICacheService> cacheService)
            : base(context, cacheService) { }

        public Task<WhiteLabelRequest?> GetByProviderIdAsync(long providerId)
        {
            return BaseQuery().FirstOrDefaultAsync(x => x.ProviderId == providerId);
        }
    }
}
