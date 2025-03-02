using Int20h2025.Auth.Interfaces;
using Int20h2025.BLL.Interfaces;
using Int20h2025.BLL.Services;
using Int20h2025.Common.Enums;
using TrelloDotNet;

namespace Int20h2025.BLL.Factories
{
    public class TaskManagerFactory(TrelloClient trelloClient, IUserContextService userContextService, HttpClient httpClient)
    {
        public ITaskManager GetTaskManager(TaskManagersEnum taskManager)
        {
            return taskManager switch
            {
                TaskManagersEnum.AzureDevOps => new AzureDevOpsService(userContextService, httpClient),
                TaskManagersEnum.Trello => new TrelloService(trelloClient),
                _ => throw new NotImplementedException("Task manager is not implemented by factory!")
            };
        }
    }
}