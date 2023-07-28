using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace SpredMedia.CommonLibrary
{
   

    public class GenericUnitOfWork<T>  where T : DbContext, IDisposable
    {
        private bool _disposedValue;
        protected readonly T _context;
        private IDbContextTransaction _objTransaction;
        public GenericUnitOfWork(T context)
        {
            _context = context;
        }


        public async Task CreateTransaction()
        {
            _objTransaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task Commit()
        {
            await _objTransaction.CommitAsync();
        }

        public async Task Rollback()
        {
            await _objTransaction?.RollbackAsync();
            await _objTransaction.DisposeAsync();
        }


        public async Task Save()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                throw new Exception();
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _context.Dispose();
                }

                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        

        // decipher this later please ============== <><><><><><><><><><>==================
        //public IRepository<T> GetRepository<T>() where T : class
        //{
        //    if (_repositories == null)
        //        _repositories = new Dictionary<string, object>();
        //    var type = typeof(T).Name;
        //    if (!_repositories.ContainsKey(type))
        //    {
        //        var repositoryType = typeof(BaseRepository<T>);

        //        //var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), _context);
        //        var repositoryInstance = new BaseRepository<T>((MainDBContext)(DbContext)_context);
        //        //Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), _context);

        //        //var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(new Type[] { typeof(T) }), _context);
        //        _repositories.Add(type, repositoryInstance);
        //    }
        //    return (BaseRepository<T>)_repositories[type];
        //}
    }
}
