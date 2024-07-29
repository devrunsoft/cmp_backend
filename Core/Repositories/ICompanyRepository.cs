using System;
using CMPNatural.Core.Entities;
using ScoutDirect.Core.Entities;
using ScoutDirect.Core.Repositories.Base;

namespace ScoutDirect.Core.Repositories
{
    public interface ICompanyRepository : IRepository<Company, long>
    {
        Task<Company?> GetByEmailAsync(string? BusinessEmail);
    }
}

