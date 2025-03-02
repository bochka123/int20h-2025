using Azure.Storage.Blobs;
using Int20h2025.Auth;
using Int20h2025.BLL;
using Int20h2025.BLL.Settings;
using Int20h2025.Common.Helpers;
using Int20h2025.Dal.Helpers;
using Int20h2025.DAL.Context;
using Int20h2025.DAL.Helpers;
using Int20h2025.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Int20h2025.WebAPI.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInt20hServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                        .WithOrigins(configuration.GetValue<string>("AllowedOrigins") ?? "")
                        .AllowCredentials()
                        .AllowAnyMethod()
                    .AllowAnyHeader());
            });
            services.AddHttpClient();
            services.Configure<AppSettings>(configuration);
            services.AddSingleton(sp => sp.GetRequiredService<IOptions<AppSettings>>().Value.Auth);
            services.AddScoped<IMigrationHelper, MigrationHelper>();
            services.AddScoped<IDatabaseSeeder, DatabaseSeeder>();
            services.AddDbContext<Int20h2025Context>(options => options.UseSqlServer(configuration.GetConnectionString("Int20h2025")));
            services.AddAuth();
            services.AddAuthDbContext<Int20h2025Context>();

            services.ConfigureBllServiceCollection();

            return services;
        }

        public static IServiceCollection AddInt20h2025AzureBlobStorage(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration["AzureBlobStorageSettings:ConnectionString"];

            services.AddScoped(_ =>
                    new BlobServiceClient(connectionString));

            services.AddSingleton(provider =>
            {
                var options = new BlobContainerOptionsHelper
                {
                    BlobContainerName = configuration["AzureBlobStorageSettings:BlobContainerName"]!
                };
                return options;
            });

            return services;
        }
    }
}
