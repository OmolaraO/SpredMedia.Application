using System;
namespace SpredMedia.Notification.Core.Interfaces
{
	public interface IGenericRepository<T>
	{
		Task DeleteAsync(string id);
		void DeleteRangeAsync(IEnumerable<T> entities);
		Task InsertAsync(T entity);
		Task<T> UpdateAsync(T entity, object key);
	}
}

