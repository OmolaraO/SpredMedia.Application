using System;
using Microsoft.EntityFrameworkCore.Storage;
using SpredMedia.Notification.Core.Interfaces;

namespace SpredMedia.Notification.Infrastructure.Repository
{
	public class UnitOfWork : IUnitOfWork, IDisposable
	{
        private bool _disposedValue;
        private readonly NotificationDbContext _context;
        private IDbContextTransaction _objTransaction;
        IEmailRepository _emailRepository;
        ISmsRepository _smsRepository;
        IUserRepository _userRepository;

        public UnitOfWork(NotificationDbContext context)
        {
            _context = context;
        }

        public IUserRepository User => _userRepository ??= new UserRepository(_context);
        public IEmailRepository Email => _emailRepository ??= new EmailRepository(_context);
        public ISmsRepository Sms => _smsRepository ??= new SmsRepository(_context);

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
            await _objTransaction.RollbackAsync();
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
    }
}

