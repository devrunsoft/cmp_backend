using BazaroApp.Core.UOW;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BazaroApp.Core.Repositories.Async
{
    public class RepositoryAsync<TEntity> : IRepositoryAsync<TEntity> where TEntity : class
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly DbSet<TEntity> _dbSet;
        public RepositoryAsync(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _dbSet = _unitOfWork.Context.Set<TEntity>();
        }

        public void DeleteEntity(TEntity entity)
        {
            if (entity != null)
                _dbSet.Remove(entity);
        }

        public async Task DeleteEntityAsync(object entityId)
        {
            TEntity entity = await _dbSet.FindAsync(entityId);
            DeleteEntity(entity);
        }

        public async Task<IEnumerable<TEntity>> GetAllEntitiesAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetEntityByAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public async Task<TEntity> GetOneEntityAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.Where(predicate).FirstOrDefaultAsync();
        }

        public async Task InsertEntityAsync(TEntity entity)
        {
            if (entity != null)
                await _dbSet.AddAsync(entity);
        }

        public async Task UpdateEntityAsync(object entityId, TEntity entity)
        {
            if (entity != null)
                _unitOfWork.Context.Entry(entity).State = EntityState.Modified;
        }
    }
}
