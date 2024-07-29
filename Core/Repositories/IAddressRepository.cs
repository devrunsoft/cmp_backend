using Bazaro.Core.Entities;
using Bazaro.Core.Repositories.Base;

namespace Bazaro.Core.Repositories
{
    public interface IAddressRepository : IRepository<Address,long>
    {
        Task<Address> GetAddressByIdAsync(long id);
        Task<IEnumerable<Address>> GetAddressByCustomerIdAsync(long customerId);
        Task<IEnumerable<Address>> GetAddressByShopIdAsync(long customerId, long shopId);
    }
}

 