using System;
using CMPNatural.Core.Entities;
using ScoutDirect.Core.Repositories.Base;

namespace CMPNatural.Core.Repositories
{

    public interface IAdminRepository : IRepository<AdminEntity, long>
    {
    }
}

