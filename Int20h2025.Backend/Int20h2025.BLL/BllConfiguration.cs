using Int20h2025.BLL.Mappers;
using Microsoft.Extensions.DependencyInjection;

namespace Int20h2025.BLL
{
    public static class BllConfiguration
    {
        public static IServiceCollection ConfigureBllServiceCollection(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MapperProfile));
            return services;
        }
    }
}
