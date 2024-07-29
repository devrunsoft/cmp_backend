using Bazaro.Core.Base;
using Bazaro.Core.Entities;
using Bazaro.Core.Repositories.Base;

namespace Bazaro.Core.Repositories
{
    public interface IShopInfoRepository : IRepository<ShopInfo, int>
    {
        Task<ShopInfo?> GetShopInfoByIdAsync(int id);
        Task<ShopInfo?> GetShopProfileByIdAsync(int id);

        Task<List<ShopInfo>> GetAllSearchShopAsync(PagedQueryRequest pagingParam, int ShopTypeId, double sLatitude, double sLongitude);
        Task<ShopInfo?> AddOrUpdateNewSearchShopByIdAsync(int id);
    }
}
