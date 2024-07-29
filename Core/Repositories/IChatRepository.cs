using Bazaro.Core.Base;
using Bazaro.Core.Entities;
using Bazaro.Core.Repositories.Base;

namespace Bazaro.Core.Repositories
{
    public interface IChatRepository : IRepository<Chat, long>
    {
        Task<IReadOnlyList<Chat>> GetInboxChatPagedAsync(long inboxId, PagedQueryRequest pagingParam);
        Task<Chat?> GetChatByIdAsync(long chatId);
        Task<List<Chat>> GetAllUnSeenChatsListAsync(long inboxId);
        Task<List<Chat>> GetAllChatIdsListAsync(long[] chatIds);
    }
}
