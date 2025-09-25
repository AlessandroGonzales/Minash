using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Domain.Repositories;

namespace Infrastructure.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<MinashDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("MinashDB"), npgsqlOptionsAction =>
                {
                    npgsqlOptionsAction.EnableRetryOnFailure();
                })
            );
            services.AddScoped<IServiceRepository, ServiceRepository>();

            return services;
        }
        
    }
}
