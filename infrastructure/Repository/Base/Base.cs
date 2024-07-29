
using ScoutDirect.Core.Repositories.Base;
using infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using ScoutDirect.Core.Base;
using ScoutDirect.Core.Caching;
using ScoutDirect.Core.Entities.Base;
using System.Linq.Expressions;

namespace ScoutDirect.infrastructure.Repository
{
    public class Repository<T, E> : IRepository<T, E> where T : class, IIdentityObject<E>
        //where T : IIdentityObject<E>
    {
        protected readonly static CacheTech cacheTech = CacheTech.Memory;
        protected readonly ScoutDBContext _dbContext;

        protected readonly Func<CacheTech, ICacheService> _cacheService;
        protected readonly string cacheKey = $"{typeof(T)}";
        public Repository(ScoutDBContext dbContext
            , Func<CacheTech, ICacheService> cacheService)
        {
            _dbContext = dbContext;
            _cacheService = cacheService;
        }
        public async Task<T> AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task AddRangeAsync(List<T> entity)
        {
            await _dbContext.Set<T>().AddRangeAsync(entity);
            await _dbContext.SaveChangesAsync();

        }
        public async Task DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }
        public async Task DeleteRangeAsync(List<T> entity)
        {
            _dbContext.Set<T>().RemoveRange(entity);
            await _dbContext.SaveChangesAsync();
        }
        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> expression)
        {
            return await _dbContext.Set<T>().Where(expression).ToListAsync();
        }

        public async Task<T> GetByIdAsync(E id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<IReadOnlyList<T>> GetPagedAsync(PagedQueryRequest pagingParam, Expression<Func<T, bool>> expression)
        {
            var skip = (pagingParam.Page - 1) * pagingParam.Size;
            return await _dbContext.Set<T>().Where(expression)
                .OrderByDescending(p => p.Id).Skip(skip).Take(pagingParam.Size).ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetPagedAsync(PagedQueryRequest pagingParam)
        {
            var skip = (pagingParam.Page - 1) * pagingParam.Size;
            return await _dbContext.Set<T>()
                .OrderByDescending(p => p.Id).Skip(skip).Take(pagingParam.Size).ToListAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _dbContext.Set<T>().Update(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}
