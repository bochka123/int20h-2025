using Int20h2025.BLL.Interfaces;
using Int20h2025.Common.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace Int20h2025.BLL.Factories
{
    public class CredsRegisterFactory(IServiceProvider serviceProvider)
    {
        public IIntegrationAuthService RegisterCreds(TaskManagersEnum taskManager)
        {
            return taskManager switch
            {
                TaskManagersEnum.Trello => serviceProvider.GetRequiredService<ITrelloAuthService>(),
                _ => throw new NotImplementedException($"Task manager {taskManager} is not implemented!")
            };
        }
    }
}
