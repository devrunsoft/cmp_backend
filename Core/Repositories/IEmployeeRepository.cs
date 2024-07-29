using Barbara.Core.Entities;
using Barbara.Core.Repositories.Base;

namespace Barbara.Core.Repositories
{
    public interface IEmployeeRepository : IRepository<Employee, long>
    {
        Task<IEnumerable<Employee>> GetEmployeeByLastName(string lastname);
    }
}
