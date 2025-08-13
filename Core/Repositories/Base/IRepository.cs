using ScoutDirect.Core.Base;
using System.Linq.Expressions;
using CMPNatural.Core.Base;

namespace ScoutDirect.Core.Repositories.Base
{

    //public interface IPagedRepository<T, Entity> where T : IPersistentObject<Entity>
    //{
    //    Task<IReadOnlyList<T>> GetPagedAsync(PagedQueryRequest pagingParam);
    //}

    //public interface IQueryPagedRepository<T, Entity> where T : IPersistentObject<Entity>
    //{
    //    Task<IReadOnlyList<T>> GetPagedAsync(PagedQueryRequest pagingParam, Expression<Func<T, bool>> expression);

    //    Task<IReadOnlyList<T>> GetPagedAsync(PagedQueryRequest pagingParam);
    //}

    public interface IRepository<T,E> where T : class
    {
        Task<IReadOnlyList<T>> GetAllAsync();
        IReadOnlyList<T> GetAllsync();
        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> expression);
        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> expression, Func<IQueryable<T>, IQueryable<T>> includeFunc = null);
        Task<IReadOnlyList<T>> GetPagedAsync(PagedQueryRequest pagingParam, Expression<Func<T, bool>> expression);
        Task<IReadOnlyList<T>> GetPagedAsync(PagedQueryRequest pagingParam);
        Task<PagesQueryResponse<T>> GetBasePagedAsync(PagedQueryRequest pagingParam, Expression<Func<T, bool>> expression, Func<IQueryable<T>, IQueryable<T>> includeFunc = null);
        Task<PagesQueryResponse<T>> GetBasePagedAsync(PagedQueryRequest pagingParam, Func<IQueryable<T>, IQueryable<T>> includeFunc = null);

        Task<TValue> GetOrCreateAsync<TValue>(
         string key, Func<Task<TValue>> factory, TimeSpan? ttl = null);
        Task<T> GetByIdAsync(E id);
        Task<T> AddAsync(T entity);
        Task AddRangeAsync(List<T> entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task DeleteRangeAsync(List<T> entity);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    }   
}
