
using Microsoft.Extensions.DependencyInjection;

namespace SpredMedia.CommonLibrary
{
    public static class AddCorsSetting
    {
        public static void AddCorsSettings(this IServiceCollection services)
        {
            services.AddCors(o =>
            {
                o.AddPolicy("AllowAll", builder =>
                    builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    );
            });
        }
    }
}
