using System;
using System.Linq;
using System.Linq.Expressions;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using Google.Protobuf.Compiler;
using infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using ScoutDirect.Core.Caching;
using ScoutDirect.infrastructure.Repository;

namespace CMPNatural.infrastructure.Repository
{

    public class OperationalAddressRepository : Repository<OperationalAddress, long>, IOperationalAddressRepository
    {
        public OperationalAddressRepository(ScoutDBContext context, Func<CacheTech, ICacheService> cacheService) : base(context, cacheService) { }

        public async Task<IEnumerable<OperationalAddress>?> GetWithChild(Expression<Func<OperationalAddress, bool>> expression)
        {
            return await _dbContext.OperationalAddress.Where(expression)
                .Include(p=>p.LocationCompany)
                .ToListAsync();
        }

    }
}

