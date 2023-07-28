using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SpredMedia.Authentication.Model.model;
using SpredMedia.CommonLibrary;
using System.Reflection.Emit;

namespace SpredMedia.Authentication.Infrastructure
{
    public class AuthenticationDbContext : IdentityDbContext<User>
    {
        public AuthenticationDbContext(DbContextOptions<AuthenticationDbContext> options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer("Server=database;Database=SpredMediaAuthenticationLocal;User=SA;Password=Nono_080;");
        }
        protected override void OnModelCreating(ModelBuilder model)
        {
            model.Entity<Client>()
                .HasMany(s => s.Endpoints)
                .WithMany(c => c.Clients)
                .UsingEntity<ClientEndpoint>(
                    j => j.HasOne(sc => sc.EndPoint)
                        .WithMany()
                        .HasForeignKey(sc => sc.EndPointId),
                    j => j.HasOne(sc => sc.client)
                        .WithMany()
                        .HasForeignKey(sc => sc.ClientID),
                    j =>
                    {
                        j.HasKey(sc => new { sc.EndPointId, sc.ClientID });
                        j.ToTable("ClientEndPoints");
                    });
            base.OnModelCreating(model);
        }
        public DbSet<User> User { get; set; }
        public DbSet<Client> Clients { get; set; }  
        public DbSet<ClientEndpoint> ClientEndpoints { get; set; }
        public DbSet<EndPoint> Endpoints { get; set; }
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var item in ChangeTracker.Entries())
            {
                if (item.Entity is User appUser)
                {
                    UpdateDateTimeContext.AuditPropertiesChange(item.State, appUser);
                }
                if (item.Entity is Client client)
                {
                    UpdateDateTimeContext.AuditPropertiesChange(item.State, client);
                }
                if (item.Entity is EndPoint endpoint)
                {
                    UpdateDateTimeContext.AuditPropertiesChange(item.State, endpoint);
                }
            }
            return await base.SaveChangesAsync(cancellationToken);
        }

        
    }
}

