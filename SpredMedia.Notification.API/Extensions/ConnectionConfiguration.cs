using System;
using Microsoft.EntityFrameworkCore;
using SpredMedia.Notification.Infrastructure;

namespace SpredMedia.Notification.API.Extensions
{
	public static class ConnectionConfiguration
	{
        public static void AddDbContextAndConfigurations(this IServiceCollection services, IWebHostEnvironment env, IConfiguration config, IConfigurationBuilder configbuild)
        {
            services.AddDbContextPool<NotificationDbContext>(options =>
            {
                string connStr;
                if (env.IsProduction())
                {
                    connStr = Environment.GetEnvironmentVariable("DefaultConnection");
                }
                else
                {
                    configbuild.AddEnvironmentVariables().AddJsonFile("appsettings.Development.json");
                    connStr = config.GetConnectionString("DefaultConnection");
                }
                options.UseSqlServer(connStr);
            });
        }
    }
}

