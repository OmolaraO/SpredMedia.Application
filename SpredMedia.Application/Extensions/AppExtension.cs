using System;
using SpredMedia.CommonLibrary;

namespace SpredMedia.UserManagement.API.Extensions
{
    public static class AppExtension
    {
        public static void UseSwaggerExtensions(this IApplicationBuilder app)
        {
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/UserManagement/swagger.json", "UserManagement Service API V1");
            });
        }

        public static void UseGlobalErrorHandlerMiddleWare(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionalMiddleware>();
        }
    }
}

