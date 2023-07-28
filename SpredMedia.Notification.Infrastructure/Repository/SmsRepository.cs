using System;
using SpredMedia.Notification.Core.Interfaces;
using SpredMedia.Notification.Model.Entity;

namespace SpredMedia.Notification.Infrastructure.Repository
{
    public class SmsRepository : GenericRepository<SMS>, ISmsRepository
    {
        public SmsRepository(NotificationDbContext context) : base(context)
        {

        }
    }
}

