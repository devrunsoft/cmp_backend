using System;
using System.Linq.Expressions;
using CMPNatural.Core.Entities;
using ScoutDirect.Core.Repositories.Base;

namespace CMPNatural.Core.Repositories
{
    public interface IOperationalAddressRepository : IRepository<OperationalAddress, long>
    {
        public Task<IEnumerable<OperationalAddress>?> GetWithChild(Expression<Func<OperationalAddress, bool>> expression);

    }
}

