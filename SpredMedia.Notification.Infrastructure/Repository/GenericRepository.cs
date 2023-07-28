using System;
using Microsoft.EntityFrameworkCore;
using SpredMedia.Notification.Core.Interfaces;

namespace SpredMedia.Notification.Infrastructure.Repository
{
	public class GenericRepository<T> : IGenericRepository<T> where T : class
	{
        private readonly NotificationDbContext _context;
        private readonly DbSet<T> _dbSet;
		public GenericRepository(NotificationDbContext context)
		{
            _context = context;
            _dbSet = _context.Set<T>();
		}

        /// <summary>
        /// Delete from database using ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task DeleteAsync(string id)
        {
            _dbSet.Remove(await _dbSet.FindAsync(id));
        }

        /// <summary>
        /// Delete Range of data from the database
        /// </summary>
        /// <param name="entities"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void DeleteRangeAsync(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        /// <summary>
        /// Insert data to the database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task InsertAsync(T entity)
        {
            try
            {
                await _dbSet.AddAsync(entity);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Update Database
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<T> UpdateAsync(T entity, object key)
        {
            if (entity is null)
            {
                return null;
            }
            T? exist = await _dbSet.FindAsync(key);
            if (exist != null)
            {
                _context.Entry(exist).CurrentValues.SetValues(entity);
            }
            return exist;
        }
    }
}

