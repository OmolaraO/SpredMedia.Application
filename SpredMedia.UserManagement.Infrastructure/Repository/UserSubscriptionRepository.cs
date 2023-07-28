using System;
using Microsoft.EntityFrameworkCore;
using SpredMedia.UserManagement.Core.Interfaces;
using SpredMedia.UserManagement.Model.Entity;

namespace SpredMedia.UserManagement.Infrastructure.Repository
{
    public class UserSubscriptionRepository : GenericRepository<UserSubscription>, IUserSubscriptionRepository
    {
        private readonly UserManagementDbContext _dbContext;

        public UserSubscriptionRepository(UserManagementDbContext context) : base(context)
        {
            _dbContext = context;
        }

        /// <summary>
        /// Gets user subscriptiond by id from the database
        /// </summary>
        /// <param name="id">ID of user subscription</param>
        /// <returns>Returns user if found and null if not found</returns>
        public async Task<UserSubscription?> GetUserSubscriptionById(string id)
        {
            return await _dbContext.UserSubscriptions.FirstOrDefaultAsync(w => w.SubscriptionId == id);
        }

        /// <summary>
        /// Gets a list of all the user subscriptions
        /// </summary>
        /// <param name=""></param>
        /// <returns>Returns the list if found and null if not found</returns>
        public async Task<List<UserSubscription>> GetAllUserSubscriptions()
        {
            return await _dbContext.UserSubscriptions.ToListAsync();
        }
    }
}

