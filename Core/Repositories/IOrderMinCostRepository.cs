using Bazaro.Core.Entities;
using Bazaro.Core.Repositories.Base;

namespace Bazaro.Core.Repositories
{
    public interface IOrderMinPriceRepository : IRepository<OrderMinPrice, int>
    {
        Task<IEnumerable<OrderMinPrice>> GetOrderMinPricesByShopIdAsync(int shopId);
        Task SetOrderMinCostByShopIdAndCostAsync(int shopId, string cost);
    }
}
