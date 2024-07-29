using Bazaro.Core.Base;
using Bazaro.Core.Entities;
using Bazaro.Core.Repositories.Base;

namespace Bazaro.Core.Repositories
{
    public interface ICustomerRepository : IRepository<Customer, long>
    {
        IQueryable<Customer> GetAllCustomersQuery();
        Task<Customer?> GetCustomerProfileByIdAsync(long Id);
        Task<IEnumerable<Customer>> GetAllCustomersAsync(PagedQueryRequest pagingParam);
        Task<Customer?> GetCustomerByIdAsync(long customerId);
    }
}
