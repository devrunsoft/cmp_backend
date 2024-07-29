using Bazaro.Core.Entities;
using Bazaro.Core.Repositories.Base;

namespace Bazaro.Core.Repositories
{
    public interface IDiscountRepository : IRepository<Discount, long>
    {
        //Task<IEnumerable<Discount>> GetAllDiscountListAsync();
        Task<IEnumerable<Discount>> GetDiscountByShopIdAsync(int shopId);
        Task<int> CreateDiscountAsync(Discount discount);
    }
}
