using System.Linq.Expressions;

namespace BazaroApp.Core.Repositories.Async
{
    public interface IRepositoryAsync<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAllEntitiesAsync();
        Task<IEnumerable<TEntity>> GetEntityByAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> GetOneEntityAsync(Expression<Func<TEntity, bool>> predicate);
        Task InsertEntityAsync(TEntity entity);
        Task UpdateEntityAsync(object entityId, TEntity entity);
        Task DeleteEntityAsync(object entityId);
        void DeleteEntity(TEntity entity);
    }
}
