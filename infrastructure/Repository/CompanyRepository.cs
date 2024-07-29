using System;
using Barbara.Core.Entities;
using CMPNatural.Core.Entities;
using infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using ScoutDirect.Core.Caching;
using ScoutDirect.Core.Entities;
using ScoutDirect.Core.Repositories;

namespace ScoutDirect.infrastructure.Repository
{
    public class CompanyRepository : Repository<Company, long>, ICompanyRepository
    {
        public CompanyRepository(ScoutDBContext context, Func<CacheTech, ICacheService> cacheService) : base(context, cacheService) { }

        public Task<Company?> GetByEmailAsync(string? BusinessEmail)
        {
            return _dbContext.Company.Where(p => p.BusinessEmail == BusinessEmail).FirstOrDefaultAsync();
        }
    }
}

