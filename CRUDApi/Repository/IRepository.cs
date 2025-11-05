using System.Linq.Expressions;

namespace CRUDApi.Repository
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(int id);
        Task<T> Add(T entity);
        Task<T> Update(T entity);
        Task<T> Delete(T entity);
        Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate);
        Task<bool> Exists(Expression<Func<T, bool>> predicate);
        Task<(IEnumerable<T> Data, int TotalRecords)> GetPagedAsync(Expression<Func<T, bool>> filter, string sortBy, string sortOrder, int pageNumber, int pageSize);
    }
}
