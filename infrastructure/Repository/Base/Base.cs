
using System;
using ScoutDirect.Core.Repositories.Base;
using infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using ScoutDirect.Core.Base;
using ScoutDirect.Core.Caching;
using ScoutDirect.Core.Entities.Base;
using System.Linq.Expressions;
using CMPNatural.Core.Base;

namespace ScoutDirect.infrastructure.Repository
{
    public class Repository<T, E> : IRepository<T, E> where T : class, IIdentityObject<E>
        //where T : IIdentityObject<E>
    {
        protected readonly static CacheTech cacheTech = CacheTech.Memory;
        protected readonly ScoutDBContext _dbContext;

        protected readonly Func<CacheTech, ICacheService> _cacheService;
        protected readonly string cacheKey = $"{typeof(T)}";

        protected ICacheService Cache => _cacheService(cacheTech);

        protected string Key(params object[] parts)
            => $"{cacheKey}:{string.Join(":", parts)}";

        private static readonly TimeSpan DefaultTtl = TimeSpan.FromMinutes(5);

        public Repository(ScoutDBContext dbContext
            , Func<CacheTech, ICacheService> cacheService)
        {
            _dbContext = dbContext;
            _cacheService = cacheService;
        }

        private IQueryable<T> BaseQuery()
        {
            return _dbContext.Set<T>().Where(x => x.IsDelete == null);
        }

        public async Task<TValue> GetOrCreateAsync<TValue>(
         string key, Func<Task<TValue>> factory, TimeSpan? ttl = null)
        {
            Cache.TryGet<TValue>(key , out TValue cached);
            if (cached is not null) return cached;

            var value = await factory();
            if (value is not null)
               Cache.Set<TValue>(key, value);

            return value;
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
            entity.IsDelete = DateTime.UtcNow;
            _dbContext.Set<T>().Update(entity);
            await _dbContext.SaveChangesAsync();
        }
        public async Task DeleteRangeAsync(List<T> entity)
        {
            foreach (var item in entity)
            {
                item.IsDelete = DateTime.UtcNow;
            }
            _dbContext.Set<T>().UpdateRange(entity);
            await _dbContext.SaveChangesAsync();
        }
        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await BaseQuery().ToListAsync();
        }

        public IReadOnlyList<T> GetAllsync()
        {
            return BaseQuery().ToList();
        }

        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> expression)
        {
            return await BaseQuery().Where(expression).ToListAsync();
        }
        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> expression, Func<IQueryable<T>, IQueryable<T>> includeFunc = null)
        {
            var query = BaseQuery();

            if (includeFunc != null)
            {
                query = includeFunc(query);
            }

            return await query.Where(expression).ToListAsync();
        }

        public async Task<T> GetByIdAsync(E id)
        {
            return await BaseQuery().FirstOrDefaultAsync(x => x.Id!.Equals(id));
        }

        public async Task<IReadOnlyList<T>> GetPagedAsync(PagedQueryRequest pagingParam, Expression<Func<T, bool>> expression)
        {
            var query = BaseQuery();


            if (!string.IsNullOrWhiteSpace(pagingParam.allField))
            {
                query = ApplySearchFilter(query, pagingParam.allField);
            }


            var skip = (pagingParam.Page - 1) * pagingParam.Size;
            return await query.Where(expression)
                .OrderByDescending(p => p.Id).Skip(skip).Take(pagingParam.Size).ToListAsync();
        }


        private IQueryable<T> ApplySearchFilter<T>(IQueryable<T> query, string searchValue)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var properties = typeof(T).GetProperties()
                .Where(p =>
                (p.PropertyType == typeof(string) ||
                (p.PropertyType.IsValueType &&
                (!p.PropertyType.IsEnum && !(Nullable.GetUnderlyingType(p.PropertyType)?.IsEnum ?? false)))) &&
                p.CanRead && p.CanWrite // Exclude read-only computed properties
                );

            Expression? finalExpression = null;
            foreach (var property in properties)
            {
                var propertyExpression = Expression.Property(parameter, property);

                Expression? convertedExpression;
                if (property.PropertyType == typeof(string))
                {
                    convertedExpression = propertyExpression;
                }
                else
                {
                    var toStringMethod = property.PropertyType.GetMethod("ToString", Type.EmptyTypes);
                    if (toStringMethod == null) continue;

                    convertedExpression = Expression.Call(propertyExpression, toStringMethod);
                }

                var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                var searchExpression = Expression.Call(convertedExpression!, containsMethod!, Expression.Constant(searchValue));

                finalExpression = finalExpression == null
                    ? searchExpression
                    : Expression.OrElse(finalExpression, searchExpression);
            }

            if (finalExpression == null) return query;

            var lambda = Expression.Lambda<Func<T, bool>>(finalExpression, parameter);
            return query.Where(lambda);
        }


        public async Task<PagesQueryResponse<T>> GetBasePagedAsync(PagedQueryRequest pagingParam, Expression<Func<T, bool>> expression, Func<IQueryable<T>, IQueryable<T>> includeFunc = null, bool filterAll=true)
        {
            IQueryable<T> query = BaseQuery();

            if (expression != null)
            {
                query = query.Where(expression);
            }

            if (includeFunc != null)
            {
                query = includeFunc(query);
            }

            if (!string.IsNullOrWhiteSpace(pagingParam.allField) && filterAll)
            {
                query = ApplySearchFilter(query, pagingParam.allField);
            }

            var totalElements = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalElements / (double)pagingParam.Size);

            var skip = (pagingParam.Page) * pagingParam.Size;
            var elements = await query
                //.Where(expression)
                .OrderByDescending(p => p.Id)
                .Skip(skip)
                .Take(pagingParam.Size)
                .ToListAsync();

            return new PagesQueryResponse<T>(elements, pagingParam.Page, totalPages, totalElements);
        }

        public async Task<PagesQueryResponse<T>> GetBasePagedAsync(PagedQueryRequest pagingParam, Func<IQueryable<T>, IQueryable<T>> includeFunc = null, bool filterAll = true)
        {
            var query = BaseQuery();

            if (includeFunc != null)
            {
                query = includeFunc(query);
            }


            // Apply search filter if `allField` is provided
            if (!string.IsNullOrWhiteSpace(pagingParam.allField) && filterAll)
            {
                query = ApplySearchFilter(query, pagingParam.allField);
            }


            var totalElements = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalElements / (double)pagingParam.Size);

            var skip = (pagingParam.Page) * pagingParam.Size;
            var elements = await query
                .OrderByDescending(p => p.Id)
                .Skip(skip)
                .Take(pagingParam.Size)
                .ToListAsync();

            return new PagesQueryResponse<T>(elements, pagingParam.Page, totalPages, totalElements);
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

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return _dbContext.SaveChangesAsync();
        }
    }
}
