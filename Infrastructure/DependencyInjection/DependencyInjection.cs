using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.ExternalServices;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Domain.Repositories;
using Polly;
using Polly.Extensions.Http;


namespace Infrastructure.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("PostgreSQL");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("PostgreSQL Connection String is missing. Check Azure App Service Key Vault references (DB_HOST, DB_USER, etc.).");
            }

            services.AddDbContext<MinashDbContext>(options =>
                options.UseNpgsql(connectionString, npgsqlOptionsAction =>
                {
                     npgsqlOptionsAction.EnableRetryOnFailure();
                }));

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

            var retryPolicy = HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(
                    retryCount: 3,
                    sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

            var circuitBreakerPolicy = HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(
                    handledEventsAllowedBeforeBreaking: 5,
                    durationOfBreak: TimeSpan.FromSeconds(30));

            services.AddHttpClient<MercadoPagoClient>()
                    .AddPolicyHandler(retryPolicy)
                    .AddPolicyHandler(circuitBreakerPolicy);

            return services;
        }

    }
}
