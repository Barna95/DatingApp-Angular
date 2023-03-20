using API.Data;
using Serilog;
using Microsoft.EntityFrameworkCore;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<AppDbContext>(options => options
             .UseSqlServer(builder.Configuration
             .GetConnectionString("DefaultConnectionString")));

            builder.Host.UseSerilog((ctx, lc) => lc
                .WriteTo.Console()
                // if you want everything, change Warning() to Information()
                .WriteTo.File("D:\\DateApp\\log.txt").MinimumLevel.Warning()
                .WriteTo.File("D:\\DateApp\\structuredLog.json").MinimumLevel.Warning());

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}