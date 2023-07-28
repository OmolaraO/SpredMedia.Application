using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using SpredMedia.Notification.Model.Entity;
using SpredMedia.CommonLibrary;

namespace SpredMedia.Notification.Infrastructure
{
	public class NotificationDbContext : DbContext
	{
        public NotificationDbContext(DbContextOptions<NotificationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Email> Emails { get; set; }
        public DbSet<SMS> Sms { get; set; }
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var item in ChangeTracker.Entries())
            {
                if (item.Entity is BaseModel entity)
                {
                    switch (item.State)
                    {
                        case EntityState.Modified:
                            entity.UpdatedAt = DateTime.UtcNow;
                            break;
                        case EntityState.Added:
                            entity.CreatedAt = entity.UpdatedAt = DateTime.UtcNow;
                            break;
                        default:
                            break;
                    }
                }
            }
            return await base.SaveChangesAsync(cancellationToken);
        }

    }
}

