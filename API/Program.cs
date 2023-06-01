using API.Data;
using API.Extensions;
using API.Middleware;
using API.SignalR;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // Add services to the container.
            builder.Services.AddApplicationServices(builder.Configuration);
            builder.Services.AddIdentityServices(builder.Configuration);
            builder.Services.AddControllers();

            builder.Host.UseSerilog((ctx, lc) => lc
                .WriteTo.Console()
                // if everything is needed to be logged, change Warning() to Information()
                .WriteTo.File("D:\\DateApp\\log.txt").MinimumLevel.Warning()
                .WriteTo.File("D:\\DateApp\\structuredLog.json").MinimumLevel.Warning());

            var connString = "";

            if (builder.Environment.IsDevelopment())
                connString = builder.Configuration.GetConnectionString("DefaultConnection");
            else
            {
                // Use connection string provided at runtime by FlyIo.
                var connUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

                // Parse connection URL to connection string for Npgsql
                connUrl = connUrl.Replace("postgres://", string.Empty);
                var pgUserPass = connUrl.Split("@")[0];
                var pgHostPortDb = connUrl.Split("@")[1];
                var pgHostPort = pgHostPortDb.Split("/")[0];
                var pgDb = pgHostPortDb.Split("/")[1];
                var pgUser = pgUserPass.Split(":")[0];
                var pgPass = pgUserPass.Split(":")[1];
                var pgHost = pgHostPort.Split(":")[0];
                var pgPort = pgHostPort.Split(":")[1];
                var updatedHost = pgHost.Replace("flycast", "internal");

                connString = $"Server={updatedHost};Port={pgPort};User Id={pgUser};Password={pgPass};Database={pgDb};";
                }
                   builder.Services.AddDbContext<AppDbContext>(opt =>
                {
                   opt.UseNpgsql(connString);
                });


            var app = builder.Build();

            app.UseMiddleware<ExceptionMiddleware>();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();
            //allow credentials for signalR
            //app.UseCors(corsBuilder =>
            //    corsBuilder.AllowAnyHeader()
            //        .AllowAnyMethod()
            //        .AllowCredentials()
            //        .WithOrigins("https://localhost:4200"));

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseDefaultFiles(); //look for index.html in wwwroot and serve by default
            app.UseStaticFiles(); //look for wwwroot and serve the content from inside there by default

            app.MapControllers();
            //create endpoint for SignalR, and which class it uses
            app.MapHub<PresenceHub>("hubs/presence");
            app.MapHub<MessageHub>("hubs/message");
            app.MapFallbackToController("Index", "Fallback");

            Seed.SeedUsers(app);

            app.Run();
        }
    }
}