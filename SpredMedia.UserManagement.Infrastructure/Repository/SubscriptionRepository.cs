
using SpredMedia.CommonLibrary;
using SpredMedia.UserManagement.Core.Interface;
using SpredMedia.UserManagement.Model.Entity;

namespace SpredMedia.UserManagement.Infrastructure.Repository
{
    public class SubscriptionRepository : GenericRepository<Subscription, UserManagementDbContext>, ISubscriptionRepository
    {
        public SubscriptionRepository(UserManagementDbContext context) : base(context) 
        {
        }
    }
}
