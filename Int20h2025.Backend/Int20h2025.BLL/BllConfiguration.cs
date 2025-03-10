﻿using Azure;
using Azure.AI.OpenAI;
using Int20h2025.BLL.Factories;
using Int20h2025.BLL.Helpers;
using Int20h2025.BLL.Interfaces;
using Int20h2025.BLL.Mappers;
using Int20h2025.BLL.Services;
using Int20h2025.BLL.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OpenAI.Chat;
using TrelloDotNet;

namespace Int20h2025.BLL
{
    public static class BllConfiguration
    {
        public static IServiceCollection ConfigureBllServiceCollection(this IServiceCollection services)
        {
            services.AddScoped<ITrelloAuthService, TrelloAuthService>();
            services.AddScoped<CredsRegisterFactory>();
            services.AddScoped<TaskManagerFactory>();
            services.AddScoped<AiHelper>();
            services.AddScoped<IRequestProcessingService, RequestProcessingService>();
            services.ConfigureAiClient();
            services.AddScoped<IAiService, AiService>();
            services.AddScoped<IProfileService, ProfileService>();
            services.AddScoped<IPromptService, PromptService>();
            services.AddScoped<IIntegrationService, IntegrationSystem>();
            services.AddAutoMapper(typeof(MapperProfile));
            return services;
        }

        public static void ConfigureAiClient(this IServiceCollection services)
        {
            services.AddSingleton<ChatClient>(x =>
            {
                var appSettings = x.GetRequiredService<IOptions<AppSettings>>().Value;
                var azureAiClient = new AzureOpenAIClient(new Uri(appSettings.Ai.ApiUrl),
                            new AzureKeyCredential(appSettings.Ai.ApiKey)).GetChatClient(appSettings.Ai.Model);

                return azureAiClient;
            });
        }
    }
}
