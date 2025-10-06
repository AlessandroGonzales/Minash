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
            services.AddScoped<IGarmentRepository, GarmentRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IGarmentServiceRepository, GarmentServiceRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IDetailsOrderRepository, DetailsOrderRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<ICustomRepository, CustomRepository>();
            services.AddScoped<IAccountingRecordRepository, AccountingRecordRepository>();
            return services;
        }
        
    }
}
