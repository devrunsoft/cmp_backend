using System;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories.Chat;
using CMPNatural.Core.Repositories.ChatCommon;
using infrastructure.Data;
using ScoutDirect.Core.Caching;
using ScoutDirect.infrastructure.Repository;

namespace CMPNatural.infrastructure.Repository.Chat
{
    public class ChatCommonMessageRepository : Repository<ChatCommonMessage, long>, IChatCommonMessageRepository
    {
        public ChatCommonMessageRepository(ScoutDBContext context, Func<CacheTech, ICacheService> cacheService)
            : base(context, cacheService) { }
    }
}

