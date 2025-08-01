using System;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories.Chat;
using infrastructure.Data;
using ScoutDirect.Core.Caching;
using ScoutDirect.infrastructure.Repository;

namespace CMPNatural.infrastructure.Repository.Chat
{
    public class ChatMentionRepository : Repository<ChatMention, long>, IChatMentionRepository
    {
        public ChatMentionRepository(ScoutDBContext context, Func<CacheTech, ICacheService> cacheService)
            : base(context, cacheService) { }
    }
}

