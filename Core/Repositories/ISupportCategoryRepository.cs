using Bazaro.Core.Base;
using Bazaro.Core.Entities;
using Bazaro.Core.Repositories.Base;

namespace Bazaro.Core.Repositories
{
    public interface ISupportCategoryRepository : IRepository<SupportCategory, int>
    {
        //Task<IEnumerable<SupportCategory>> GetAllSupportCategoryAsync();
        //Task<SupportCategory> GetSupportCategoryByIdAsync(int id);
        Task<IEnumerable<SupportCategory>> GetAllSupportCategoryByParentIdAsync(PagedQueryRequest pagining, bool isShop, int? parentId);
        Task<List<int?>> GetAllParentIdsAsync(); 
    }
}
