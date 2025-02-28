using Int20h2025.BLL.Interfaces;
using Int20h2025.BLL.Mappers;
using Int20h2025.BLL.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Int20h2025.BLL
{
    public static class BllConfiguration
    {
        public static IServiceCollection ConfigureBllServiceCollection(this IServiceCollection services)
        {
            services.AddScoped<IProfileService, ProfileService>();
            services.AddAutoMapper(typeof(MapperProfile));
            return services;
        }
    }
}
