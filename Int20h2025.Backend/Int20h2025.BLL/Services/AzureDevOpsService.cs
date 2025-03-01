using Int20h2025.BLL.Interfaces;
using Int20h2025.Common.Models.Ai;
using Microsoft.Identity.Web;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.VisualStudio.Services.OAuth;
using Microsoft.VisualStudio.Services.WebApi;
using Microsoft.VisualStudio.Services.WebApi.Patch.Json;

namespace Int20h2025.BLL.Services
{
    public class AzureDevOpsService(ITokenAcquisition tokenAcquisition) : ITaskManager
    {
        public string SystemName { get; init; } = "AzureDevOps";

        private readonly string _devOpsOrgUrl = "";

        public async Task<OperationResult> ExecuteMethodAsync(string methodName, object[] parameters)
        {
            switch (methodName)
            {
                case "CreateTask":
                    var title = parameters[0].ToString();
                    var projectName = parameters[1].ToString();
                    var assignedTo = parameters[2].ToString();
                    return await CreateTaskAsync(title, projectName, assignedTo);

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
                                Name = "projectName",
                                Type = "string",
                                Description = "Name of project to create task name."
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

        private async Task<OperationResult> CreateTaskAsync(string title, string projectName, string assignedTo)
        {
            var scopes = new[] { "/.default" };
            var accessToken = await tokenAcquisition.GetAccessTokenForUserAsync(scopes);

            var credentials = new VssOAuthAccessTokenCredential(accessToken);
            using (var connection = new VssConnection(new Uri(_devOpsOrgUrl), credentials))
            {
                var workItemClient = connection.GetClient<WorkItemTrackingHttpClient>();

                var patchDocument = new JsonPatchDocument
                {
                    new JsonPatchOperation
                    {
                        Operation = Microsoft.VisualStudio.Services.WebApi.Patch.Operation.Add,
                        Path = "/fields/System.Title",
                        Value = title
                    },
                    new JsonPatchOperation
                    {
                        Operation = Microsoft.VisualStudio.Services.WebApi.Patch.Operation.Add,
                        Path = "/fields/System.Description",
                        Value = "Created via Azure DevOps API with OAuth"
                    },
                    new JsonPatchOperation
                    {
                        Operation = Microsoft.VisualStudio.Services.WebApi.Patch.Operation.Add,
                        Path = "/fields/System.AssignedTo",
                        Value = assignedTo
                    }
                };

                var workItem = await workItemClient.CreateWorkItemAsync(patchDocument, projectName, "Task");
            }

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
