using Int20h2025.BLL.Interfaces;
using Int20h2025.BLL.Services;
using Int20h2025.Common.Enums;

namespace Int20h2025.BLL.Factories
{
    public class TaskManagerFactory
    {
        public ITaskManager GetTaskManager(TaskManagersEnum taskManager)
        {
            return taskManager switch 
            {
                TaskManagersEnum.AzureDevOps => new AzureDevOpsService(),
                _ => throw new NotImplementedException("Task manager is not implemented by factory!")
            };
        }
    }
}