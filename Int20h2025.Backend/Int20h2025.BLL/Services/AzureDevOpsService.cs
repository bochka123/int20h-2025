using Int20h2025.BLL.Interfaces;
using Int20h2025.Common.Models.Ai;

namespace Int20h2025.BLL.Services
{
    public class AzureDevOpsService : ITaskManager
    {
        public string SystemName { get; init; } = "AzureDevOps";

        public async Task<OperationResult> ExecuteMethodAsync(string methodName, object[] parameters)
        {
            switch (methodName)
            {
                case "CreateTask":
                    var title = parameters[0].ToString();
                    var assignedTo = parameters[1].ToString();
                    return await CreateTaskAsync(title, assignedTo);

                case "UpdateTask":
                    var parseUpdateTaskIdsuccess = int.TryParse(parameters[0].ToString(), out var updateTaskId);

                    if (!parseUpdateTaskIdsuccess)
                    {
                        throw new InvalidCastException("Couldn` convert updateTaskId from AI");
                    }

                    var status = parameters[1].ToString();
                    return await UpdateTaskAsync(updateTaskId, status);

                case "DeleteTask":
                    var parseDeleteTaskIdsuccess = int.TryParse(parameters[0].ToString(), out var deleteTaskId);

                    if (!parseDeleteTaskIdsuccess)
                    {
                        throw new InvalidCastException("Couldn` convert deleteTaskId from AI");
                    }

                    var filePath = parameters[1].ToString();
                    return await DeleteTaskAsync(deleteTaskId);

                default:
                    throw new NotImplementedException($"Method '{methodName}' is not implemented.");
            }
        }

        public SystemMethodInfo GetAvailableMethods()
        {
            return new SystemMethodInfo
            {
                SystemName = SystemName,
                Methods =
                    [
                    new ServiceMethodInfo
                    {
                        MethodName = "CreateTask",
                        Description = "Creates a new task in the specified project.",
                        Parameters =
                        [
                            new ParameterInfo
                            {
                                Name = "title",
                                Type = "string",
                                Description = "Task name."
                            },
                            new ParameterInfo
                            {
                                Name = "assignedTo",
                                Type = "string",
                                Description = "Email of user to be assigned."
                            }
                        ]
                    },
                    new ServiceMethodInfo
                    {
                        MethodName = "UpdateTask",
                        Description = "Update task by id and status that contains new status.",
                        Parameters =
                        [
                            new ParameterInfo
                            {
                                Name = "taskId",
                                Type = "int",
                                Description = "Id of task to be updated."
                            },
                            new ParameterInfo
                            {
                                Name = "status",
                                Type = "string",
                                Description = "New status that needs to be updated."
                            }
                        ]
                    },
                    new ServiceMethodInfo
                    {
                        MethodName = "DeleteTask",
                        Description = "Deletes task by id.",
                        Parameters =
                        [
                            new ParameterInfo
                            {
                                Name = "taskId",
                                Type = "int",
                                Description = "Id of task to be deleted."
                            }
                        ]
                    }
                ]
            };
        }

        private async Task<OperationResult> CreateTaskAsync(string title, string assignedTo)
        {
            return new OperationResult { Response = $"Задачу '{title}' створено та призначено {assignedTo}.", Success = true };
        }

        private async Task<OperationResult> UpdateTaskAsync(int taskId, string status)
        {
            return new OperationResult { Response = $"Задачу з номером '{taskId}' оновлено та призначено статус {status}.", Success = true };
        }

        private async Task<OperationResult> DeleteTaskAsync(int taskId)
        {
            return new OperationResult { Response = $"Задачу з номером '{taskId}' видалено.", Success = true };
        }
    }
}
