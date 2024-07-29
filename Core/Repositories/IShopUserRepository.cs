using Bazaro.Core.Entities;
using Bazaro.Core.Repositories.Base;

namespace Bazaro.Core.Repositories
{
    public interface IShopUserRepository : IRepository<ShopUser, long>
    {
        Task<ShopUser?> GetShopUserAsync(int id);
        Task<IEnumerable<Tuple<string, ShopUser>>> GetShopUsers(int shopId);
        Task<IEnumerable<ShopUser>> GetShopAssistant(int shopId);
        Task<ShopUser?> GetShopUserByPersonId(long personId, bool includeShop);
        Task<IEnumerable<ShopUser>> GetShopUsers(long shopId);
        Task<IEnumerable<ShopUser>> GetAllShopUsers();
    }
}
