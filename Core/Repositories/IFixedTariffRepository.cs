using Bazaro.Core.Base;
using Bazaro.Core.Entities;
using Bazaro.Core.Repositories.Base;

namespace Bazaro.Core.Repositories
{
    public interface IFixedTariffRepository : IRepository<FixedTariff, int>
    {
        Task<IReadOnlyList<FixedTariff>> GetPagedFixedTariffAsync(PagedQueryRequest pagingParam);
    }
}
