using API.Data;
using API.Interfaces;
using API.Services;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddEndpointsApiExplorer();
            services.AddScoped<ITokenService, TokenService>();

            services.AddDbContext<AppDbContext>(options => options
                .UseSqlServer(config
                    .GetConnectionString("DefaultConnectionString")));

            services.AddCors();

            return services;
        }
    }
}
