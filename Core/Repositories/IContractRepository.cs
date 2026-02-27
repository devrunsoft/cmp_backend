using System;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Enums;
using ScoutDirect.Core.Repositories.Base;

namespace CMPNatural
{
    public interface IContractRepository : IRepository<Contract, long>
    {
       public Task UnsetDefaultForOthersAsync(long excludeId, ContractType Type);
    }
}

