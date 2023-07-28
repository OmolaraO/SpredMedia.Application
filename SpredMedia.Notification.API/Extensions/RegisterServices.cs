using System;
using SpredMedia.Notification.Core.AppSettings;
using SpredMedia.Notification.Core.Interfaces;
using SpredMedia.Notification.Core.Services;
using SpredMedia.Notification.Core.Utilities;
using SpredMedia.Notification.Infrastructure.ExternalServices;
using SpredMedia.Notification.Infrastructure.Repository;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace SpredMedia.Notification.API.Extensions
{
	public static class RegisterServices
	{
		public static void AddRegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IEmailNotificationProvider, EmailNotificationProvider>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}

