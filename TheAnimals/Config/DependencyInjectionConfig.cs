using Microsoft.AspNetCore.Mvc.DataAnnotations;
using TheAnimals.Interfaces;
using TheAnimals.Services;

namespace Ipet.Configurations
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ResolveDependencies(this IServiceCollection services)
        {
            services.AddScoped<IDatabaseService, DatabaseService>();

            return services;
        }
    }
}