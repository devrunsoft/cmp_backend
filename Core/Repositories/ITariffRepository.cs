using Bazaro.Core.Base;
using Bazaro.Core.Entities;
using Bazaro.Core.Repositories.Base;

namespace Bazaro.Core.Repositories
{
    public interface ITariffRepository : IRepository<Tariff, int>
    {
        Task<IReadOnlyList<Tariff>> GetAllPagedAsync(PagedQueryRequest pagingParam);
        Task<Tariff?> GetTariffByIdAsync(int TariffId);
    }
}
