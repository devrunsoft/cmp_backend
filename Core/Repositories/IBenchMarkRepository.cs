using Bazaro.Core.Entities;
using Bazaro.Core.Repositories.Base;

namespace Bazaro.Core.Repositories
{
    public interface IBenchmarkRepository : IRepository<BenchMark, int>
    {
        Task<IEnumerable<BenchMark>> GetBenchmarksByFormKey(string formKey);
    }
}
