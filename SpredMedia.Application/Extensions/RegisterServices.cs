using System;
using SpredMedia.UserManagement.Core.Interfaces;
using SpredMedia.UserManagement.Core.Services;
using SpredMedia.UserManagement.Infrastructure.ExternalServices;
using SpredMedia.UserManagement.Infrastructure.Repository;

namespace SpredMedia.UserManagement.API.Extensions
{
	public static class RegisterServices
	{
        public static void AddRegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IProfileServices, ProfileServices>();
            services.AddScoped<IHistoryServices, HistoryServices>();
            services.AddScoped<IImageServices, ImageServices>();
            services.AddScoped<ICloudinaryServices, CloudinaryServices>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}

