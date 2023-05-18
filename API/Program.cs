using API.Data;
using API.Extensions;
using API.Middleware;
using API.SignalR;
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


            var app = builder.Build();

            app.UseMiddleware<ExceptionMiddleware>();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();
            //allow credentials for signalR
            app.UseCors(corsBuilder =>
                corsBuilder.AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
                    .WithOrigins("https://localhost:4200"));

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();
            //create endpoint for SignalR, and which class it uses
            app.MapHub<PresenceHub>("hubs/presence");
            app.MapHub<MessageHub>("hubs/message");

            Seed.SeedUsers(app);

            app.Run();
        }
    }
}