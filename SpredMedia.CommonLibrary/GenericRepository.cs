
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace SpredMedia.CommonLibrary
{
    public class GenericRepository<T, U> : IGenericRepository<T> where T : class where U : DbContext
    {
        private readonly U _context;
        private readonly DbSet<T> _db;
        public GenericRepository(U context)
        {
            _context = context;
            _db = _context.Set<T>();
        }
        /// <summary>
        /// Deletes an Object
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteAsync(string id)
        {
            _db.Remove(await _db.FindAsync(id));
        }
        /// <summary>
        /// Deletes List of objects
        /// </summary>
        /// <param name="entities"></param>
        public void DeleteRangeAsync(IEnumerable<T> entities)
        {
            _db.RemoveRange(entities);
        }
        /// <summary>
        /// Inserts An Object
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task InsertAsync(T entity)
        {
            try
            {
                await _db.AddAsync(entity);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Update(T item)
        {
            // attaches instance to the contex
            // t, then sets the state
            // as modified
            _db.Attach(item);
            _context.Entry(item).State = EntityState.Modified;
        }
        /// <summary>
        /// get all item in the database
        /// return a defered function;
        /// </summary>
        /// <returns>IQueryable<T></returns>
        public IQueryable<T> GetAllAsync()
        {
            return _db;
        }
        /// <summary>
        /// get all the value that meet the requirement of the predicate
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public async Task<T> GetAsync(System.Linq.Expressions.Expression<Func<T, bool>> expression, List<string> includes = null)
        {
            IQueryable<T> query = _db;
            if (includes != null)
            {
                foreach (var property in includes)
                {
                    query = query.Include(property);
                }
            }

            return await _db.AsNoTracking().FirstOrDefaultAsync(expression);
        }

    }
}
