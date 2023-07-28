using System;
using SpredMedia.UserManagement.Model.Entity;

namespace SpredMedia.UserManagement.Core.Interfaces
{
	public interface IUserSubscriptionRepository : IGenericRepository<UserSubscription>
	{
        Task<UserSubscription?> GetUserSubscriptionById(string id);
        Task<List<UserSubscription>> GetAllUserSubscriptions();
    }
}

