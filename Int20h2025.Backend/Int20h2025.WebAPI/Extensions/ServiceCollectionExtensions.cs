using Azure.Storage.Blobs;
using Int20h2025.Auth;
using Int20h2025.BLL;
using Int20h2025.BLL.Interfaces;
using Int20h2025.BLL.Services;
using Int20h2025.BLL.Settings;
using Int20h2025.Common.Helpers;
using Int20h2025.Dal.Helpers;
using Int20h2025.DAL.Context;
using Int20h2025.DAL.Helpers;
using Int20h2025.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;

namespace Int20h2025.WebAPI.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInt20hServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMicrosoftIdentityWebApiAuthentication(configuration, "AzureAd")
                .EnableTokenAcquisitionToCallDownstreamApi()
                .AddInMemoryTokenCaches();

            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.CustomOperationIds(e => $"{e.ActionDescriptor.RouteValues["controller"]}_{e.ActionDescriptor.RouteValues["action"]}");
                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    In = ParameterLocation.Header,
                    Scheme = "Bearer",
                    Flows = new OpenApiOAuthFlows
                    {
                        Implicit = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri($"{configuration["AzureAd:Instance"]}common/oauth2/v2.0/authorize"),
                            Scopes = new Dictionary<string, string> { { "api://bobrintelligence_services/user_impersonation", "Default" } }
                        }
                    }
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "oauth2"
                                }
                            },
                            new string[] {}
                        }
                });
            });
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
            services.ConfigureBllServiceCollection();

            services.AddAuth();
            services.AddAuthDbContext<Int20h2025Context>();

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
