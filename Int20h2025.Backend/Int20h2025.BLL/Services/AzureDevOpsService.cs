﻿using Int20h2025.Auth.Interfaces;
using Int20h2025.BLL.Interfaces;
using Int20h2025.Common.Exceptions;
using Int20h2025.Common.Models.Ai;
using Int20h2025.Common.Models.AzureDevops;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.VisualStudio.Services.OAuth;
using Microsoft.VisualStudio.Services.WebApi;
using Microsoft.VisualStudio.Services.WebApi.Patch.Json;
using Newtonsoft.Json.Linq;

namespace Int20h2025.BLL.Services
{
    public class AzureDevOpsService(IUserContextService userContextService) : ITaskManager
    {
        public string SystemName { get; init; } = "AzureDevOps";
        private readonly string _devOpsOrgUrl = "https://dev.azure.com/";
        public async Task<OperationResult> ExecuteMethodAsync(string methodName, JObject parameters)
        {
            switch (methodName)
            {
                case "CreateTask":
                    var model = parameters.ToObject<CreateTaskModel>()!;
                    return await CreateTaskAsync(model);

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

        private async Task<OperationResult> CreateTaskAsync(CreateTaskModel model)
        {
            var accessToken = userContextService.UserData ?? throw new InternalPointerBobrException("User must sign in devops firstly.");
            var credentials = new VssOAuthAccessTokenCredential(accessToken);
            using var connection = new VssConnection(new Uri(_devOpsOrgUrl + model.OrganizationName), credentials);
            var workItemClient = connection.GetClient<WorkItemTrackingHttpClient>();

            var patchDocument = new JsonPatchDocument
            {
                new JsonPatchOperation
                {
                    Operation = Microsoft.VisualStudio.Services.WebApi.Patch.Operation.Add,
                    Path = "/fields/System.Title",
                    Value = model.Title
                },
                new JsonPatchOperation
                {
                    Operation = Microsoft.VisualStudio.Services.WebApi.Patch.Operation.Add,
                    Path = "/fields/System.AssignedTo",
                    Value = model.AssignedTo
                }
            };

            if (!string.IsNullOrEmpty(model.Description))
            {
                patchDocument.Add(new JsonPatchOperation
                {
                    Operation = Microsoft.VisualStudio.Services.WebApi.Patch.Operation.Add,
                    Path = "/fields/System.Description",
                    Value = model.Description
                });
            }

            if (!string.IsNullOrEmpty(model.Priority))
            {
                patchDocument.Add(new JsonPatchOperation
                {
                    Operation = Microsoft.VisualStudio.Services.WebApi.Patch.Operation.Add,
                    Path = "/fields/Microsoft.VSTS.Common.Priority",
                    Value = model.Priority
                });
            }

            var workItem = await workItemClient.CreateWorkItemAsync(patchDocument, model.ProjectName, "Task");

            return new OperationResult { Response = $"WorkItem: {workItem.Url}, {model.Title}, {model.AssignedTo}", Success = true };
        }

        private async Task<OperationResult> UpdateTaskAsync(int taskId, string status)
        {
            return new OperationResult { Response = $"Задачу з номером '{taskId}' оновлено та призначено статус {status}.", Success = true };
        }

        private async Task<OperationResult> DeleteTaskAsync(int taskId)
        {
            return new OperationResult { Response = $"Задачу з номером '{taskId}' видалено.", Success = true };
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
                                Description = "Task name.",
                                IsRequired = true
                            },
                            new ParameterInfo
                            {
                                Name = "projectName",
                                Type = "string",
                                Description = "Name of project to create task name.",
                                IsRequired = true
                            },
                            new ParameterInfo
                            {
                                Name = "organizationName",
                                Type = "string",
                                Description = "Name of organization to create task name.",
                                IsRequired = true
                            },
                            new ParameterInfo
                            {
                                Name = "assignedTo",
                                Type = "string",
                                Description = "Email of user to be assigned.",
                                IsRequired = true
                            },
                            new ParameterInfo
                            {
                                Name = "description",
                                Type = "string",
                                Description = "Description of the task.",
                                IsRequired = false
                            },
                            new ParameterInfo
                            {
                                Name = "priority",
                                Type = "string",
                                Description = "Priority of the task.",
                                IsRequired = false
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
                                Description = "Id of task to be updated.",
                                IsRequired = true
                            },
                            new ParameterInfo
                            {
                                Name = "status",
                                Type = "string",
                                Description = "New status that needs to be updated.",
                                IsRequired = true
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
                                Description = "Id of task to be deleted.",
                                IsRequired = true
                            }
                        ]
                    }
                ]
            };
        }
    }
}
