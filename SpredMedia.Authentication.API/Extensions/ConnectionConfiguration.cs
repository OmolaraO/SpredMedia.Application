using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SpredMedia.Authentication.Core.Services;
using SpredMedia.Authentication.Infrastructure;
using SpredMedia.Authentication.Model.model;

namespace SpredMedia.Authentication.API.Extensions
{
    public static class ConnectionConfiguration
    {
        public static void AddDbContextAndConfigurations(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<AuthenticationDbContext>(options =>
            {
                options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            });

            var builder = services.AddIdentity<User, IdentityRole>(x =>
            {
                x.Password.RequiredLength = 8;
                x.Password.RequireDigit = false;
                x.Password.RequireUppercase = true;
                x.Password.RequireLowercase = true;
                x.User.RequireUniqueEmail = true;
                x.SignIn.RequireConfirmedEmail = true;
            });

            builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole), services);
            _ = builder.AddEntityFrameworkStores<AuthenticationDbContext>()
            .AddTokenProvider<DigitalTokenService>(DigitalTokenService.DIGITEMAIL)
            .AddTokenProvider<DigitalTokenService>(DigitalTokenService.DIGITPHONE)
            .AddDefaultTokenProviders();
        }
    }
}
