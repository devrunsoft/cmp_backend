using System;
using System.Linq.Expressions;
using CMPNatural.Core.Entities;
using ScoutDirect.Core.Repositories.Base;

namespace CMPNatural.Core.Repositories
{
    public interface IProductRepository : IRepository<Product, long>
    {
        public void sync();
    }
}


