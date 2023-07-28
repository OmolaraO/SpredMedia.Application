using System;
namespace SpredMedia.UserManagement.Core.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task InsertAsync(T entity);
        Task DeleteAsync(string id);
        void DeleteEntityAsync(T entity);
        void DeleteRangeAsync(IEnumerable<T> entities);
        void Update(T item);
    }
}

