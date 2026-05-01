using CMPNatural.Core.Repositories;
using infrastructure.Data;
using ScoutDirect.Core.Caching;
using ScoutDirect.infrastructure.Repository;
using CMPNatural.Core.Entities;

namespace CMPNatural.infrastructure.Repository
{
    public class NotificationRepository : Repository<Notification, long>, INotificationRepository
    {
        public NotificationRepository(ScoutDBContext context, Func<CacheTech, ICacheService> cacheService) : base(context, cacheService) { }
    }
}

