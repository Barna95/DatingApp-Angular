using API.Extensions;
using API.Middleware;
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

            app.UseCors(corsBuilder => corsBuilder.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200"));

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}