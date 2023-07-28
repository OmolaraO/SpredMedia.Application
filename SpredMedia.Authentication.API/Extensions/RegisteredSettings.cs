using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SpredMedia.Authentication.Core.AppSettings;

namespace SpredMedia.Authentication.API.Extensions
{
    public static class RegisteredSettings
    {
        public static void AddAppSettings(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<NotificationSettings>(config.GetSection(nameof(NotificationSettings)));
            services.AddScoped(cfg => cfg.GetRequiredService<IOptions<NotificationSettings>>().Value);
        }
    }
}
