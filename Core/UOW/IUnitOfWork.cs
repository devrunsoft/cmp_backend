using BazaroApp.Core.Data;
using BazaroApp.Core.Repositories.Async;
using BazaroApp.Core.Repositories.Sync;
using Microsoft.EntityFrameworkCore;

namespace BazaroApp.Core.UOW
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;
        IRepositoryAsync<TEntity> GetRepositoryAsync<TEntity>() where TEntity : class;
        ApplicationDbContext Context { get; }
        int Save();
        Task<int> SaveAsync();
    }

    public interface IUnitOfWork<TContext> : IUnitOfWork where TContext : DbContext
    {
    }
}
