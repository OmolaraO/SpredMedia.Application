using System;
namespace SpredMedia.Notification.Core.Interfaces
{
	public interface IUnitOfWork
	{
        IUserRepository User { get; }
        IEmailRepository Email { get; }
        ISmsRepository Sms { get; }
        Task Commit();
        Task CreateTransaction();
        void Dispose();
        Task Rollback();
        Task Save();
    }
}

