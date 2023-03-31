using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Builder;
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
                context.Database.EnsureCreated();

                var userData =  await File.ReadAllTextAsync("Data/UserSeedData.json");

                if (!context.Users.Any())
                {
                    var users = JsonSerializer.Deserialize<List<AppUser>>(userData);

                    foreach (var user in users)
                    {
                        using var hmac = new HMACSHA512();
                        user.UserName = user.UserName.ToLower();
                        user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$w0rd"));
                        user.PasswordSalt = hmac.Key;

                        context.Users.Add(user);
                    }

                    await context.SaveChangesAsync();
                }
            }
        }
    }
}

