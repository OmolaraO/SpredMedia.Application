using System;
namespace SpredMedia.UserManagement.Core.Interfaces
{
	public interface IUnitOfWork
	{
        IUserRepository User { get; }
        IProfileRepository UserProfile { get; }
        IDownloadHistoryRepository DownloadHistory { get; }
        IViewingHistoryRepository ViewingHistory { get; }
        IUserSubscriptionRepository UserSubscription { get; }

        Task Commit();
        Task CreateTransaction();
        void Dispose();
        Task Rollback();
        Task Save();
    }
}

