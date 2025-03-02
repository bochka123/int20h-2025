using Int20h2025.Auth.Interfaces;
using Int20h2025.BLL.Interfaces;
using Int20h2025.Common.Enums;
using Int20h2025.Common.Exceptions;
using Int20h2025.Common.Models.DTO.IntegrationSystem;
using Int20h2025.DAL.Context;
using Microsoft.EntityFrameworkCore;

namespace Int20h2025.BLL.Services
{
    public class IntegrationSystem(Int20h2025Context context, IUserContextService userContextService) : IIntegrationService
    {
        public async Task IntegrateUserAsync(TaskManagersEnum integrationSystem)
        {
            var userId = userContextService.UserId;
            var system = await context.Systems.FirstOrDefaultAsync(x => x.Name == integrationSystem.ToString()) ?? throw new InternalPointerBobrException($"Integration with {integrationSystem.ToString()} cannot be created yet");
            var integration = await context.Integrations.FirstOrDefaultAsync(x => x.ProfileId == userId && x.System.Name == system.Name);

            if (integration == null)
            {
                integration = new DAL.Entities.Integration()
                {
                    ProfileId = userId,
                    SystemId = system.Id,
                    AccessToken = userContextService.UserData,
                    IsConnected = true

                };

                await context.Integrations.AddAsync(integration);
                await context.SaveChangesAsync();
            }
        }

        public async Task IntegrateUserAsync(IntegrationSystemDTO integrationSystem)
        {
            var userId = userContextService.UserId;
            var system = await context.Systems.FirstOrDefaultAsync(x => x.Name == integrationSystem.SystemName.ToString()) ?? throw new InternalPointerBobrException($"Integration with {integrationSystem.ToString()} cannot be created yet");
            var integration = await context.Integrations.FirstOrDefaultAsync(x => x.ProfileId == userId && x.System.Name == system.Name);

            if (integration == null)
            {
                integration = new DAL.Entities.Integration()
                {
                    ProfileId = userId,
                    SystemId = system.Id,
                    AccessToken = userContextService.UserData,
                    IsConnected = true,
                    ApiKey = integrationSystem.ApiKey,
                    Token = integrationSystem.Token

                };

                await context.Integrations.AddAsync(integration);
            }
            else
            {
                integration.AccessToken = userContextService.UserData;
                integration.ApiKey = integrationSystem.ApiKey;
                integration.Token = integrationSystem.Token;

                context.Integrations.Update(integration);
            }

            await context.SaveChangesAsync();
        }
    }
}
