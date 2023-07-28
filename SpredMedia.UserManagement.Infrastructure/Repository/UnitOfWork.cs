using System;
using Microsoft.EntityFrameworkCore.Storage;
using SpredMedia.UserManagement.Core.Interfaces;
using SpredMedia.UserManagement.Model.Entity;

namespace SpredMedia.UserManagement.Infrastructure.Repository
{
	public class UnitOfWork : IUnitOfWork
	{
        private bool _disposedValue;
        private readonly UserManagementDbContext _context;
        private IDbContextTransaction _objTransaction;
        IUserRepository _userRepository { get; set; } 
        IProfileRepository _profileRepository { get; set; } = null!;
        IDownloadHistoryRepository _downloadHistoryRepository { get; set; } = null!;
        IViewingHistoryRepository _viewHistoryRepository { get; set; } = null!;
        IUserSubscriptionRepository _userSubscriptionRepository { get; set; } = null!;

        public UnitOfWork(UserManagementDbContext context)
        {
            _context = context;
        }

        public IUserRepository User => _userRepository ??= new UserRepository(_context);
        public IProfileRepository UserProfile => _profileRepository ??= new ProfileRepository(_context);
        public IDownloadHistoryRepository DownloadHistory => _downloadHistoryRepository ??= new DownloadHistoryRepository(_context);
        public IViewingHistoryRepository ViewingHistory => _viewHistoryRepository ??= new ViewingHistoryRepository(_context);
        public IUserSubscriptionRepository UserSubscription => _userSubscriptionRepository ??= new UserSubscriptionRepository(_context);

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

