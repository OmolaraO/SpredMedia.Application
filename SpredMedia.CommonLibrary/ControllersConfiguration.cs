using Microsoft.Extensions.DependencyInjection;
using FluentValidation.AspNetCore;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace SpredMedia.CommonLibrary
{
    public static class ControllersConfiguration
    {
        [Obsolete]
        public static void AddControllersExtension(this IServiceCollection services)
        {
            services.AddControllers()
            .AddFluentValidation(fv =>
            {
                fv.DisableDataAnnotationsValidation = true;
                fv.RegisterValidatorsFromAssemblyContaining<Program>();
                fv.ImplicitlyValidateChildProperties = true;
            })
            .AddNewtonsoftJson(op => op.SerializerSettings.ReferenceLoopHandling
               = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
        }
    }
}
