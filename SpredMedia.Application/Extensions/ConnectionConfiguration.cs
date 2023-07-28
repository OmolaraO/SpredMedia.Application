using System;
using Microsoft.EntityFrameworkCore;
using SpredMedia.UserManagement.Infrastructure;

namespace SpredMedia.UserManagement.API.Extensions
{
	public static class ConnectionConfiguration
	{
        public static void AddDbContextAndConfigurations(this IServiceCollection services, IWebHostEnvironment env, IConfiguration config, IConfigurationBuilder configbuild)
        {
            services.AddDbContextPool<UserManagementDbContext>(options =>
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

