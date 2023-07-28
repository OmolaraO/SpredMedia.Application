using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using Microsoft.Extensions.DependencyInjection;
using SpredMedia.Authentication.Model.model;
using SpredMedia.Authentication.Model.Enum;
using Serilog;

namespace SpredMedia.Authentication.Infrastructure
{
    public class AuthenticationMsDbInitializer
    {
        public static async Task Seed(IApplicationBuilder builder, ILogger _logger )
        {
            try
            {
                _logger.Information("about to check if the applicaton has already been seeded");
                using var serviceScope = builder.ApplicationServices.CreateScope();
                var context = serviceScope.ServiceProvider.GetService<AuthenticationDbContext>();
                string filePath = Path.Combine(AppContext.BaseDirectory, "DataSeeding");
                if (!await context.Database.EnsureCreatedAsync()) return;

                if (!context.Roles.Any())
                {
                    _logger.Information("about to seed the roles");
                    var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                    var readText = await File.ReadAllTextAsync(Path.Combine(filePath , "Roles.json"));
                    List<IdentityRole> Roles = JsonConvert.DeserializeObject<List<IdentityRole>>(readText);
                    foreach (var role in Roles)
                    {
                        await roleManager.CreateAsync(role);
                    }
                    _logger.Information("roles successfully seeded to the database");
                }
                if (!context.User.Any())
                {
                    _logger.Information("about to seed the user to the database");
                    var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<User>>();
                    var readText = await File.ReadAllTextAsync(Path.Combine(filePath , "UserSeed.json"));
                    List<User> users = JsonConvert.DeserializeObject<List<User>>(readText);
                    users.ForEach(delegate (User user)
                    {
                        userManager.CreateAsync(user, user.PasswordHash);
                        userManager.AddToRoleAsync(user, Role.Admin.ToString());
                        context.User.AddAsync(user);
                    });
                    _logger.Information("seeded the user to database successfully");
                }
                if (!context.ClientEndpoints.Any())
                {
                    _logger.Information("the clientendpint is about to seeded to the database");
                    var readText = await File.ReadAllTextAsync(Path.Combine(filePath , "ClientEndpointSeed.json"));
                    List<ClientEndpoint> ClientEnpoints = JsonConvert.DeserializeObject<List<ClientEndpoint>>(readText);
                    foreach (var ClientEnpoint in ClientEnpoints)
                    {
                        await context.ClientEndpoints.AddAsync(ClientEnpoint);
                        _logger.Information("seeded the client endpoint successfully");
                    }
                    if (!context.Clients.Any())
                    {
                        _logger.Information("about to seed the client user applicaton to the database");
                        var readClient = await File.ReadAllTextAsync(Path.Combine(filePath , "ClientSeed.json"));
                        List<Client> Clients = JsonConvert.DeserializeObject<List<Client>>(readClient);
                        foreach (var Client in Clients)
                        {
                            await context.Clients.AddAsync(Client);
                        }
                        _logger.Information("seed the client applicatoin successfully to the database ");
                    }
                    if (!context.Endpoints.Any())
                    {
                        _logger.Information("about to seed the user endpoint to the database");
                        var readEndpoint = await File.ReadAllTextAsync(Path.Combine(filePath ,"EndpointSeed.json"));
                        List<EndPoint> Endpoints = JsonConvert.DeserializeObject<List<EndPoint>>(readEndpoint);
                        foreach (var Endpoint in Endpoints)
                        {
                            await context.Endpoints.AddAsync(Endpoint);
                        }
                    }
                    await context.SaveChangesAsync();
                    _logger.Information("seeded the user endpoint to the database successfully");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, "unable to seed to database");
                _logger.Error(ex.StackTrace);
                throw;
            }
            
        }
    }
}
