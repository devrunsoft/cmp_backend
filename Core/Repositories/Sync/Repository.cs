using BazaroApp.Core.UOW;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BazaroApp.Core.Repositories.Sync
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// General repository class
        /// </summary>
        /// <param name="TEntity"></param>

        private readonly IUnitOfWork _unitOfWork;
        private readonly DbSet<TEntity> _dbSet;
        public Repository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _dbSet = _unitOfWork.Context.Set<TEntity>();
        }

        public void DeleteEntity(object entityId)
        {
            TEntity entity = _dbSet.Find(entityId);
            DeleteEntity(entity);
        }

        public void DeleteEntity(TEntity entity)
        {
            if (entity != null)
                _dbSet.Remove(entity);
        }

        public IEnumerable<TEntity> GetAllEntities()
        {
            return _dbSet.ToList();
        }

        public IEnumerable<TEntity> GetEntityBy(Expression<Func<TEntity, bool>> predicate)
        {
            IQueryable<TEntity> query = _dbSet;
            if(predicate != null)
                query = query.Where(predicate);

            return query;
        }

        public TEntity GetOneEntity(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbSet.Where(predicate).FirstOrDefault();
        }

        public void InsertEntity(TEntity entity)
        {
            if(entity != null)
                _dbSet.Add(entity);
        }

        public void UpdateEntity(object entityId, TEntity entity)
        {
            if(entity != null)
                _unitOfWork.Context.Entry(entity).State = EntityState.Modified;
        }
    }
}
