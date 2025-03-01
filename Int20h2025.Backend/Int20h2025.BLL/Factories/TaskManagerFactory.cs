using Int20h2025.BLL.Interfaces;
using Int20h2025.BLL.Services;
using Int20h2025.Common.Enums;
using Microsoft.Identity.Web;

namespace Int20h2025.BLL.Factories
{
    public class TaskManagerFactory(ITokenAcquisition tokenAcquisition)
    {
        public ITaskManager GetTaskManager(TaskManagersEnum taskManager)
        {
            return taskManager switch 
            {
                TaskManagersEnum.AzureDevOps => new AzureDevOpsService(tokenAcquisition),
                _ => throw new NotImplementedException("Task manager is not implemented by factory!")
            };
        }
    }
}