using Microsoft.EntityFrameworkCore;
using SpredMedia.CommonLibrary;
using SpredMedia.UserManagement.Model.Entity;
using System.Net;

namespace SpredMedia.UserManagement.Infrastructure
{
    public class UserManagementDbContext : DbContext
    {
        public UserManagementDbContext(DbContextOptions<UserManagementDbContext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<UserSubscription> UserSubscriptions { get; set; }
        public DbSet<DownloadHistory> DownloadHistory { get; set; }
        public DbSet<ViewingHistory> ViewingHistories { get; set; }
        public DbSet<ProfileDownloads> ProfileDownloads { get; set; }
        

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var item in ChangeTracker.Entries())
            {
                if (item.Entity is User appUser)
                {
                    UpdateDateTimeContext.AuditPropertiesChange(item.State, appUser);
                }
                else if (item.Entity is Profile profile)
                {
                    UpdateDateTimeContext.AuditPropertiesChange(item.State, profile);
                }
                else if (item.Entity is UserProfile Userprofile)
                {
                    UpdateDateTimeContext.AuditPropertiesChange(item.State, Userprofile);
                }
                else if (item.Entity is Subscription Sub)
                {
                    UpdateDateTimeContext.AuditPropertiesChange(item.State, Sub);
                }
                else if (item.Entity is DownloadHistory Download)
                {
                    UpdateDateTimeContext.AuditPropertiesChange(item.State, Download);
                }
                else if (item.Entity is ViewingHistory History)
                {
                    UpdateDateTimeContext.AuditPropertiesChange(item.State, History);
                }
            }
            return await base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Profile>()
                .HasMany(s => s.Downloads)
                .WithMany(c => c.profiles)
                .UsingEntity<ProfileDownloads>(
                    j => j.HasOne(sc => sc.DownloadHistory)
                        .WithMany()
                        .HasForeignKey(sc => sc.DownloadHistoryId),
                    j => j.HasOne(sc => sc.Profile)
                        .WithMany()
                        .HasForeignKey(sc => sc.ProfileId),
                    j =>
                    {
                        j.HasKey(sc => new { sc.DownloadHistoryId, sc.ProfileId });
                        j.ToTable("ProfileDownloads");
                    });

            modelBuilder.Entity<User>()
                .HasMany(s => s.Subscriptions)
                .WithMany(c => c.Users)
                .UsingEntity<UserSubscription>(
                    j => j.HasOne(sc => sc.Subscription)
                        .WithMany()
                        .HasForeignKey(sc => sc.SubscriptionId),
                    j => j.HasOne(sc => sc.User)
                        .WithMany()
                        .HasForeignKey(sc => sc.UserId),
                    j =>
                    {
                        j.HasKey(sc => new { sc.SubscriptionId, sc.UserId });
                        j.ToTable("UserSubscriptions");
                    });
            base.OnModelCreating(modelBuilder);
        }
    }
}


