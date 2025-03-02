using Int20h2025.Auth.Interfaces;
using Int20h2025.BLL.Interfaces;
using Int20h2025.BLL.Services;
using Int20h2025.Common.Enums;
using Int20h2025.DAL.Context;
using TrelloDotNet;

namespace Int20h2025.BLL.Factories
{
    public class TaskManagerFactory(Int20h2025Context context, ITrelloAuthService trelloAuthService, IUserContextService userContextService, HttpClient httpClient)
    {
        public ITaskManager GetTaskManager(string taskManager)
        {
            return taskManager switch
            {
                nameof(TaskManagersEnum.AzureDevOps) => new AzureDevOpsService(userContextService, httpClient),
                nameof(TaskManagersEnum.Trello) => new TrelloService(context, trelloAuthService, userContextService),
                _ => throw new NotImplementedException("Task manager is not implemented by factory!")
            };
        }
    }
}