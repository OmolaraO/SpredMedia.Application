using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SpredMedia.UserManagement.Model.Entity;

namespace SpredMedia.UserManagement.Infrastructure
{
	public class UserManagementDbInitializer
	{
        public static async Task Seed(IApplicationBuilder builder)
        {
            using var serviceScope = builder.ApplicationServices.CreateScope();
            var context = serviceScope.ServiceProvider.GetService<UserManagementDbContext>();
            string filePath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName, @"SpredMedia.UserManagement.Infrastructure\Data\");
            if (await context.Database.EnsureCreatedAsync()) return;

            if (!context.Users.Any())
            {
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<User>>();
                var readText = await File.ReadAllTextAsync(filePath + "Users.json");
                List<User> users = JsonConvert.DeserializeObject<List<User>>(readText);
                users.ForEach(delegate (User user) {
                    userManager.CreateAsync(user, "codeApe_06$");
                    //userManager.AddToRoleAsync(user);
                    context.Users.AddAsync(user);
                });
            }
            await context.SaveChangesAsync();
        }
    }
}

