using System.Linq.Expressions;

namespace SpredMedia.CommonLibrary
{
    public interface IGenericRepository<T> where T : class
    {
        Task DeleteAsync(string id);
        void DeleteRangeAsync(IEnumerable<T> entities);

        IQueryable<T> GetAllAsync();
        Task<T> GetAsync(Expression<Func<T, bool>> expression, List<string> includes = null);
        Task InsertAsync(T entity);
        void Update(T item);
    }
}