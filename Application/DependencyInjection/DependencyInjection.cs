using Application.Interfaces;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Register application services here
            services.AddScoped<IServiceAppService, ServiceAppService>();
            services.AddScoped<IGarmentAppService, GarmentAppService>();
            services.AddScoped<IRoleAppService, RoleAppService>();
            services.AddScoped<IGarmentServiceAppService, GarmentServiceAppService>();
            services.AddScoped<IUserAppService, UserAppService>();
            services.AddScoped<IOrderAppService, OrderAppService>();
            services.AddScoped<IDetailsOrderAppService, DetailsOrderAppService>();
            services.AddScoped<IPaymentAppService, PaymentAppService>();
            services.AddScoped<ICustomAppService, CustomAppService>();
            services.AddScoped<IAccountingRecordAppService, AccountingRecordAppService>();
            return services;
        }
    }
}
