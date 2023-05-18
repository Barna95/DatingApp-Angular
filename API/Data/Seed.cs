using System.Text.Json;
using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class Seed
    {
        public static async void SeedUsers(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateAsyncScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AppDbContext>();
                await context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE [Connections]");
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();
                context.Database.EnsureCreated();

                var userData =  await File.ReadAllTextAsync("Data/UserSeedData.json");

                if (!userManager.Users.Any())
                {
                    var users = JsonSerializer.Deserialize<List<AppUser>>(userData);

                    var roles = new List<AppRole>()
                    {
                        new() { Name = "Member" },
                        new() { Name = "Admin" },
                        new() { Name = "Moderator" },
                    };
                    foreach (var role in roles)
                    {
                        await roleManager.CreateAsync(role);
                    }

                    foreach (var user in users)
                    {
                        user.UserName = user.UserName.ToLower();

                        await userManager.CreateAsync(user, "Pa$$w0rd");
                        await userManager.AddToRoleAsync(user, "Member");
                    }

                    var admin = new AppUser()
                    {
                        UserName = "Admin"
                    };

                    await userManager.CreateAsync(admin, "Pa$$w0rd");
                    await userManager.AddToRolesAsync(admin, new[] { "Admin", "Moderator" });

                }
            }
        }
    }
}

