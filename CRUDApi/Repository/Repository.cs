using CRUDApi.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;

namespace CRUDApi.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DbSet<T> _dbSet;
        private readonly DeptDbContext _deptDbContext;
        public Repository(DeptDbContext deptDbContext)
        {
            _dbSet = deptDbContext.Set<T>();
            _deptDbContext = deptDbContext;
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> GetById(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<T> Add(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _deptDbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<T> Update(T entity)
        {
            _dbSet.Attach(entity);
            _deptDbContext.Entry(entity).State = EntityState.Modified;
            await _deptDbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<T> Delete(T entity)
        {
            //_dbSet.Remove(entity);
            _deptDbContext.Entry(entity).State = EntityState.Modified;
            await _deptDbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public async Task<bool> Exists(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }

        public async Task<(IEnumerable<T> Data, int TotalRecords)> GetPagedAsync(Expression<Func<T, bool>> filter, string sortBy, string sortOrder, int pageNumber, int pageSize)
        {
            var query = _dbSet.Where(filter);

            var prop = typeof(T).GetProperty(sortBy, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (prop != null)
            {
                var parameter = Expression.Parameter(typeof(T), "d");
                var propertyAccess = Expression.MakeMemberAccess(parameter, prop);
                var orderByExp = Expression.Lambda(propertyAccess, parameter);
                string methodName = sortOrder == "desc" ? "OrderByDescending" : "OrderBy";

                var resultExp = Expression.Call(typeof(Queryable), methodName, new Type[] { typeof(T), prop.PropertyType }, query.Expression, Expression.Quote(orderByExp));

                query = query.Provider.CreateQuery<T>(resultExp);
            }

            var totalRecords = await query.CountAsync();
            var data = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            return (data, totalRecords);
        }
    }
}
