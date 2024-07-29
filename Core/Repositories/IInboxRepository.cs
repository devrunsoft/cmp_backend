using Bazaro.Core.Base;
using Bazaro.Core.Entities;
using Bazaro.Core.Repositories.Base;

namespace Bazaro.Core.Repositories
{
    public interface IInboxRepository : IRepository<Inbox, long>//, IPagedRepository<Inbox, long>
    {
        Task<Inbox?> GetInbox(int shopId, long customerId, long addressId);

        IReadOnlyList<Tuple<string,Person>> GetInboxUsers(long id);
        Task<IReadOnlyList<Tuple<string, Person>>> GetInboxUsersAsync(long id);

        Task<IReadOnlyList<Inbox>> GetInbox(PagedQueryRequest pagingParam, int ShopId, long? Id);

        Task<IReadOnlyList<Inbox>> GetCustomerInbox(PagedQueryRequest pagingParam, int CustomerId, long? Id);


        void RemoveInboxCachesAsync(List<long> iboxIds);

        Task<List<long>> GetShopInboxIdsAsync(int shopId);

        Task<List<long>> GetPersonInboxIdsAsync(long personId);

        Task<List<long>> GetShopUserInboxIdsByPersonIdAsync(long personId);

        IQueryable<Inbox> GetAllInboxQuery();

        Task<Inbox?> GetDeferredOrder(long inboxId, long chatId);

    }
}
