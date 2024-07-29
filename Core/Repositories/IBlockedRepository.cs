using Bazaro.Core.Entities;
using Bazaro.Core.Repositories.Base;

namespace Bazaro.Core.Repositories
{
    public interface IBlockedRepository : IRepository<Blocked, int>
    {
        Task<Blocked?> GetBlockedAsync(long customerId,int shopId);
    }
}
