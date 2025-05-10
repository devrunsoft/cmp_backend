using System;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using ScoutDirect.Core.Caching;
using ScoutDirect.infrastructure.Repository;

namespace CMPNatural.infrastructure.Repository
{
    public class ContractRepository : Repository<Contract, long>, IContractRepository
    {
        public ContractRepository(ScoutDBContext context, Func<CacheTech, ICacheService> cacheService) : base(context, cacheService) { }

        public async Task UnsetDefaultForOthersAsync(long excludeId)
        {
            var defaultContracts = await _dbContext.Contract
                .Where(c => c.IsDefault && c.Id != excludeId)
                .ToListAsync();

            foreach (var contract in defaultContracts)
            {
                contract.IsDefault = false;
            }

            await _dbContext.SaveChangesAsync();
        }

    }
}