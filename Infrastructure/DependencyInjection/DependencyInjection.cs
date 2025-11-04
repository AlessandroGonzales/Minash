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
            var connectionString = "Host=minashpostgresql.postgres.database.azure.com;Port=5432;Database=Minash;Username=postgres@minashpostgresql;Password=[TU_PASS_REAL];Ssl Mode=Require;Trust Server Certificate=true;";

            Console.WriteLine("=== DI: Using connection = " + connectionString);

            services.AddDbContext<MinashDbContext>(options =>
         options.UseNpgsql("Host=minashpostgresql.postgres.database.azure.com;Port=5432;Database=Minash;Username=postgres@minashpostgresql;Password=[TU_PASS_REAL];Ssl Mode=Require;Trust Server Certificate=true;", npgsqlOptionsAction =>
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
