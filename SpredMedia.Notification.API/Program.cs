using System.Reflection;
using Microsoft.Extensions.Options;
using Serilog;
using SpredMedia.CommonLibrary;
using SpredMedia.Notification.API.Extensions;
using SpredMedia.Notification.Core.AppSettings;
using SpredMedia.Notification.Core.Interfaces;
using SpredMedia.Notification.Core.Services;
using SpredMedia.Notification.Core.Utilities;
using SpredMedia.Notification.Infrastructure.ExternalServices;
using SpredMedia.Notification.Infrastructure.Repository;
//using ILogger = Serilog.ILogger;


try
{
    var builder = WebApplication.CreateBuilder(args);
    // gettung the setting from the appsettings
    var config = builder.Configuration;

    // add the logger settings
    Log.Logger = SeriLogExtension.SerilogRegister(config);
    Log.Logger.Information("the Notification MS has started well");

    var serviceName = config.GetSection("MicroServices").GetValue<string>("Identifier");
    var assembleName = Assembly.GetExecutingAssembly().GetName().Name;

    builder.Services.AddDbContextAndConfigurations(builder.Environment, builder.Configuration, builder.Configuration);

    // Add services to the container.
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSingleton(Log.Logger);
    builder.Services.AddSwaggerGen();
    builder.Services.AddSwaggerConfiguration(serviceName, assembleName);
    builder.Services.AddRegisterServices();

    builder.Services.AddScoped(v => v.GetRequiredService<IOptions<NotificationSettings>>().Value);

    builder.Services.Configure<NotificationSettings>(config.GetSection(nameof(NotificationSettings)));

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwaggerExtensions();
    }
    app.UseHttpsRedirection();
    app.UseCors("AllowAll");
    app.UseMiddleware<ExceptionalMiddleware>();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Logger.Fatal(ex.StackTrace, "the application has failed to startup well");
    Log.CloseAndFlush();
}
 