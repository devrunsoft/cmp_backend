using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories.Chat;
using infrastructure.Data;
using ScoutDirect.Core.Caching;
using ScoutDirect.infrastructure.Repository;

namespace CMPNatural.infrastructure.Repository.Chat
{
    public class ChatMessageNoteRepository : Repository<ChatMessageNote, long>, IChatMessageNoteRepository
    {
        public ChatMessageNoteRepository(ScoutDBContext context, Func<CacheTech, ICacheService> cacheService)
            : base(context, cacheService) { }
    }
}

