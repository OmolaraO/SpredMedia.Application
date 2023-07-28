using Serilog;
using SpredMedia.Authentication.API.Extensions;
using SpredMedia.Authentication.Infrastructure;
using SpredMedia.CommonLibrary;
using System.Reflection;

try
{
    var builder = WebApplication.CreateBuilder(args);
    // getting the setting from the appsettings
    var config = builder.Configuration;

    // add the logger settings
    Log.Logger = SeriLogExtension.SerilogRegister(config);
    Log.Logger.Information("the Authentication MS has started well");

    var serviceName = config.GetSection("MicroServices").GetValue<string>("Identifier");
    var assembleName = Assembly.GetExecutingAssembly().GetName().Name;

    // Add services to the container.
    builder.Services.AddControllers();
    builder.Services.AddControllersExtension();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSingleton(Log.Logger);
    builder.Services.AddSwaggerGen();
    builder.Services.AddAppSettings(config);
    builder.Services.RegisterServices();
    builder.Services.AddFluentValidation();
    builder.Services.AddDbContextAndConfigurations(config);
    builder.Services.AddSwaggerConfiguration(serviceName, assembleName);
    builder.Services.AddAuthenticationExtension(config);

    var app = builder.Build();
    await AuthenticationMsDbInitializer.Seed(app, Log.Logger);

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/Authentication/swagger.json", "Authentication API V1");
        });
    }
    app.UseCors("AllowAll");
    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseMiddleware<ExceptionalMiddleware>();

    app.MapControllers();
    app.Run();
}
catch (Exception ex)
{
    Log.Logger.Fatal(ex.StackTrace, "the application has failed to startup well");
    Log.CloseAndFlush();
}

