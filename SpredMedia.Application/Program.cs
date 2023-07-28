using System.Reflection;
using Microsoft.Extensions.Options;
using Serilog;
using SpredMedia.CommonLibrary;
using System.Reflection;
using SpredMedia.UserManagement.Core.Utilities.Profiles;
using SpredMedia.UserManagement.Infrastructure;
using SpredMedia.UserManagement.API.Extensions;

try
{
    var builder = WebApplication.CreateBuilder(args);
    // getting the setting from the appsettings
    var config = builder.Configuration;

    // add the logger settings
    Log.Logger = SeriLogExtension.SerilogRegister(config);
    Log.Logger.Information("the UserManagement MS has started well");

    var serviceName = config.GetSection("MicroServices").GetValue<string>("Identifier");
    var assembleName = Assembly.GetExecutingAssembly().GetName().Name;


    // Add services to the container.
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSingleton(Log.Logger);
    builder.Services.AddSwaggerGen();
    builder.Services.AddSwaggerConfiguration(serviceName, assembleName);
    builder.Services.AddAutoMapper(typeof(MappingProfiles));
    builder.Services.AddRegisterServices();
    builder.Services.AddDbContextAndConfigurations(builder.Build().Environment,config,builder.Configuration);

    var app = builder.Build();
    await UserManagementDbInitializer.Seed(app);

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwaggerExtensions();
    }
    app.UseHttpsRedirection();
    app.UseCors("AllowAll");
    //app.UseWhiteListingConfiguration(config);
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
