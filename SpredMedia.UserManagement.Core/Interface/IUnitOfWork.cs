
using SpredMedia.CommonLibrary;

namespace SpredMedia.UserManagement.Core.Interface
{
    public interface IUnitOfWork : IGenericUnitOfWork
    {
        IDownloadHistoryRepository DownloadHistoryRepository { get; }
        IProfileDownloadRepoository ProfileDownloadRepoository { get; }
        IProfileRepository ProfileRepository { get; }
        ISubscriptionRepository SubscriptionRepository { get; }
        IUserRepository UserRepository { get; }
        IUserSubscriptionRepository UserSubscriptionRepository { get; }
    }
}
