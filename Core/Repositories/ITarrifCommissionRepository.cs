using Bazaro.Core.Entities;
using Bazaro.Core.Repositories.Base;

namespace Bazaro.Core.Repositories
{
    public interface ITarrifCommissionRepository : IRepository<Commission, int>
    {
        Task<IReadOnlyList<Commission>> GetAllCommissionAsync();
    }
}
