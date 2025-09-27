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
            return services;
        }
    }
}
