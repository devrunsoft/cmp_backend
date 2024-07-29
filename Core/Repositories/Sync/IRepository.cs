using System.Linq.Expressions;

namespace BazaroApp.Core.Repositories.Sync
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> GetAllEntities();
        IEnumerable<TEntity> GetEntityBy(Expression<Func<TEntity, bool>> predicate);
        TEntity GetOneEntity(Expression<Func<TEntity, bool>> predicate);
        void InsertEntity(TEntity entity);
        void UpdateEntity(object entityId, TEntity entity);
        void DeleteEntity(object entityId);
        void DeleteEntity(TEntity entity);
    }
}
