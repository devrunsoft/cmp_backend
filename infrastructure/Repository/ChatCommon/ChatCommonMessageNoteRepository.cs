using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories.Chat;
using infrastructure.Data;
using ScoutDirect.Core.Caching;
using ScoutDirect.infrastructure.Repository;

namespace CMPNatural.infrastructure.Repository.Chat
{
    public class ChatCommonMessageNoteRepository : Repository<ChatCommonMessageNote, long>, IChatCommonMessageNoteRepository
    {
        public ChatCommonMessageNoteRepository(ScoutDBContext context, Func<CacheTech, ICacheService> cacheService)
            : base(context, cacheService) { }
    }
}

