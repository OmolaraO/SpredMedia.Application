using SpredMedia.Authentication.Core.Interface;
using SpredMedia.Authentication.Core.Services;
using SpredMedia.Authentication.Core.Utility;
using SpredMedia.Authentication.Infrastructure.Repository;

namespace SpredMedia.Authentication.API.Extensions
{
    public static class AddRegisteredService
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IClientAccountService, ClientAccountService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IEndpointServices, EndpointServices>();
            services.AddScoped<IUserAuthService, UserAuthService>();
            services.AddScoped<IEmailer, Emailer>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IDigitalTokenService, DigitalTokenService>();
            services.AddAutoMapper(typeof(AuthenticationProfile));
            services.AddHttpContextAccessor();
            services.AddHttpClient();
            services.AddScoped<IAuthClientService, AuthHttpClient>();
        }
    }
}
