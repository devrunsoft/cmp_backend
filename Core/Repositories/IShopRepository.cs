using Bazaro.Core.Entities;
using Bazaro.Core.Repositories.Base;

namespace Bazaro.Core.Repositories
{
    public interface IShopRepository : IRepository<Shop, int>
    {
        Task<IEnumerable<Shop>> GetAllShopsAsync();
        IQueryable<Shop> GetAllShopQuery();
        Task<Shop> GetShopByIdAsync(int shopId);
        Task<IEnumerable<Shop>> GetShopByNameAsync(string shopName);
        Task<IEnumerable<Shop>> GetShopByShopPhoneAsync(string shopPhone);
        Task CreateShopAsync(Shop shop);
        Task UpdateShopAsync(Shop shop);
    }
}
