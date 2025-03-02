using Int20h2025.Auth.Interfaces;
using Int20h2025.BLL.Interfaces;
using Int20h2025.Common.Enums;
using Int20h2025.Common.Exceptions;
using Int20h2025.Common.Models.Ai;
using Int20h2025.DAL.Context;
using Int20h2025.Common.Models.AzureDevops;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;

namespace Int20h2025.BLL.Services
{
    public class AzureDevOpsService(Int20h2025Context context, IUserContextService userContextService, HttpClient httpClient) : ITaskManager
    {
        public DAL.Entities.System System => context.Systems.FirstOrDefault(x => x.Name == nameof(TaskManagersEnum.AzureDevOps))
                                            ?? throw new InternalPointerBobrException("System not configured");
        private const string _devOpsUrl = "https://dev.azure.com/";

        public async Task<OperationResult> ExecuteMethodAsync(string methodName, JObject parameters)
        {
            var integration = context.Integrations.FirstOrDefault(x => x.SystemId == System.Id && x.ProfileId == userContextService.UserId);

            if (integration == null) return new OperationResult { Response = $"User doesn't integrated with '{nameof(TaskManagersEnum.AzureDevOps)}'. Login via Microsoft account.", Success = false };

            switch (methodName)
            {
                case "CreateTask":
                    var createTaskModel = parameters.ToObject<CreateTaskModel>();
                    if (createTaskModel == null)
                        return new OperationResult { Success = false, Response = "Invalid parameters for CreateTask method." };
                    return await CreateTaskAsync(createTaskModel);

                case "UpdateTask":
                    var updateTaskModel = parameters.ToObject<UpdateTaskModel>();
                    if (updateTaskModel == null)
                        return new OperationResult { Success = false, Response = "Invalid parameters for UpdateTask method." };
                    return await UpdateTaskAsync(updateTaskModel);

                case "DeleteTask":
                    var deleteTaskModel = parameters.ToObject<DeleteTaskModel>();
                    if (deleteTaskModel == null)
                        return new OperationResult { Success = false, Response = "Invalid parameters for DeleteTask method." };
                    return await DeleteTaskAsync(deleteTaskModel);

                case "GetOrganizations":
                    return await GetOrganizationsAsync();

                case "GetProjects":
                    var organizationName = parameters["organizationName"]?.ToString();
                    if (string.IsNullOrEmpty(organizationName))
                        return new OperationResult { Success = false, Response = "Organization name is required for GetProjects method." };
                    return await GetProjectsAsync(organizationName);

                default:
                    return new OperationResult { Success = false, Response = $"Method '{methodName}' is not implemented." };
            }
        }

        public async Task<OperationResult> GetOrganizationsAsync()
        {
            var accessToken = userContextService.UserData;
            if (string.IsNullOrEmpty(accessToken))
                return new OperationResult { Success = false, Response = "User must sign in to Azure DevOps first." };

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                var response = await httpClient.GetAsync("https://app.vssps.visualstudio.com/_apis/accounts");
                if (!response.IsSuccessStatusCode)
                    return new OperationResult { Success = false, Response = $"Failed to fetch organizations. Status code: {response.StatusCode}" };

                var content = await response.Content.ReadAsStringAsync();
                var organizations = JArray.Parse(content);

                var organizationNames = organizations
                    .Select(o => o["AccountName"]?.ToString())
                    .Where(x => !string.IsNullOrEmpty(x))
                    .ToList();

                return new OperationResult
                {
                    Response = string.Join(", ", organizationNames),
                    Success = true
                };
            }
            catch (Exception ex)
            {
                return new OperationResult
                {
                    Response = $"An error occurred while fetching organizations: {ex.Message}",
                    Success = false
                };
            }
        }

        public async Task<OperationResult> GetProjectsAsync(string organizationName)
        {
            var accessToken = userContextService.UserData;
            if (string.IsNullOrEmpty(accessToken))
                return new OperationResult { Success = false, Response = "User must sign in to Azure DevOps first." };

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                var response = await httpClient.GetAsync($"{_devOpsUrl}{organizationName}/_apis/projects?api-version=7.1");
                if (!response.IsSuccessStatusCode)
                    return new OperationResult { Success = false, Response = $"Failed to fetch projects. Status code: {response.StatusCode}" };

                var content = await response.Content.ReadAsStringAsync();
                var projects = JObject.Parse(content);

                var projectNames = projects["value"]
                    .Select(p => p["name"]?.ToString())
                    .Where(x => !string.IsNullOrEmpty(x))
                    .ToList();

                return new OperationResult
                {
                    Response = string.Join(", ", projectNames),
                    Success = true
                };
            }
            catch (Exception ex)
            {
                return new OperationResult
                {
                    Response = $"An error occurred while fetching projects: {ex.Message}",
                    Success = false
                };
            }
        }

        private async Task<OperationResult> CreateTaskAsync(CreateTaskModel model)
        {
            var accessToken = userContextService.UserData;
            if (string.IsNullOrEmpty(accessToken))
                return new OperationResult { Success = false, Response = "User must sign in to Azure DevOps first." };

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var patchDocument = new[]
            {
                new
                {
                    op = "add",
                    path = "/fields/System.Title",
                    value = model.Title
                },
                new
                {
                    op = "add",
                    path = "/fields/System.AssignedTo",
                    value = model.AssignedTo
                }
            };

            if (!string.IsNullOrEmpty(model.Description))
            {
                patchDocument = patchDocument.Append(new
                {
                    op = "add",
                    path = "/fields/System.Description",
                    value = model.Description
                }).ToArray();
            }

            if (!string.IsNullOrEmpty(model.Priority))
            {
                patchDocument = patchDocument.Append(new
                {
                    op = "add",
                    path = "/fields/Microsoft.VSTS.Common.Priority",
                    value = model.Priority
                }).ToArray();
            }

            var jsonContent = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(patchDocument), Encoding.UTF8, "application/json-patch+json");

            try
            {
                var response = await httpClient.PostAsync($"{_devOpsUrl}{model.OrganizationName}/{model.ProjectName}/_apis/wit/workitems/$Task?api-version=6.0", jsonContent);
                if (!response.IsSuccessStatusCode)
                    return new OperationResult { Success = false, Response = $"Failed to create task. Status code: {response.StatusCode}" };

                var content = await response.Content.ReadAsStringAsync();
                var workItem = JObject.Parse(content);

                var resp = new TaskResponseModel
                {
                    Id = workItem["id"]!.ToObject<int>(),
                    Title = model.Title,
                    Url = workItem["_links"]["html"]["href"].ToString()
                };

                return new OperationResult { Response = resp.ToString(), Success = true };
            }
            catch (Exception ex)
            {
                return new OperationResult
                {
                    Response = $"An error occurred while creating the task: {ex.Message}",
                    Success = false
                };
            }
        }

        private async Task<OperationResult> UpdateTaskAsync(UpdateTaskModel model)
        {
            var accessToken = userContextService.UserData;
            if (string.IsNullOrEmpty(accessToken))
                return new OperationResult { Success = false, Response = "User must sign in to Azure DevOps first." };

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var patchDocument = new List<object>();

            if (!string.IsNullOrEmpty(model.Status))
            {
                patchDocument.Add(new
                {
                    op = "add",
                    path = "/fields/System.State",
                    value = model.Status
                });
            }

            if (!string.IsNullOrEmpty(model.Description))
            {
                patchDocument.Add(new
                {
                    op = "add",
                    path = "/fields/System.Description",
                    value = model.Description
                });
            }

            if (!string.IsNullOrEmpty(model.Priority))
            {
                patchDocument.Add(new
                {
                    op = "add",
                    path = "/fields/Microsoft.VSTS.Common.Priority",
                    value = model.Priority
                });
            }

            if (!string.IsNullOrEmpty(model.AssignedTo))
            {
                patchDocument.Add(new
                {
                    op = "add",
                    path = "/fields/System.AssignedTo",
                    value = model.AssignedTo
                });
            }

            var jsonContent = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(patchDocument), Encoding.UTF8, "application/json-patch+json");

            try
            {
                var response = await httpClient.PatchAsync($"{_devOpsUrl}{model.OrganizationName}/{model.ProjectName}/_apis/wit/workitems/{model.Id}?api-version=6.0", jsonContent);
                if (!response.IsSuccessStatusCode)
                    return new OperationResult { Success = false, Response = $"Failed to update task. Status code: {response.StatusCode}" };

                var content = await response.Content.ReadAsStringAsync();
                var workItem = JObject.Parse(content);

                var resp = new TaskResponseModel
                {
                    Id = workItem["id"].ToObject<int>(),
                    Url = workItem["_links"]["html"]["href"].ToString()
                };

                return new OperationResult
                {
                    Response = resp.ToString(),
                    Success = true
                };
            }
            catch (Exception ex)
            {
                return new OperationResult
                {
                    Response = $"An error occurred while updating the task: {ex.Message}",
                    Success = false
                };
            }
        }

        private async Task<OperationResult> DeleteTaskAsync(DeleteTaskModel model)
        {
            var accessToken = userContextService.UserData;
            if (string.IsNullOrEmpty(accessToken))
                return new OperationResult { Success = false, Response = "User must sign in to Azure DevOps first." };

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                var response = await httpClient.DeleteAsync($"{_devOpsUrl}{model.OrganizationName}/{model.ProjectName}/_apis/wit/workitems/{model.TaskId}?api-version=6.0");
                if (!response.IsSuccessStatusCode)
                    return new OperationResult { Success = false, Response = $"Failed to delete task. Status code: {response.StatusCode}" };

                return new OperationResult
                {
                    Response = "Task deleted successfully.",
                    Success = true
                };
            }
            catch (Exception ex)
            {
                return new OperationResult
                {
                    Response = $"An error occurred while deleting the task: {ex.Message}",
                    Success = false
                };
            }
        }

        public SystemMethodInfo GetAvailableMethods()
        {
            return new SystemMethodInfo
            {
                SystemName = System.Name,
                Methods =
                [
                    new ServiceMethodInfo
                    {
                        MethodName = "CreateTask",
                        Description = "Creates a new task in the specified project.",
                        Parameters =
                        [
                            new ParameterInfo { Name = "title", Type = "string", Description = "Task name.", IsRequired = true },
                            new ParameterInfo { Name = "projectName", Type = "string", Description = "Name of project to create task name.", IsRequired = true },
                            new ParameterInfo { Name = "organizationName", Type = "string", Description = "Name of organization to create task name.", IsRequired = true },
                            new ParameterInfo { Name = "assignedTo", Type = "string", Description = "Email of user to be assigned.", IsRequired = true },
                            new ParameterInfo { Name = "description", Type = "string", Description = "Description of the task.", IsRequired = false },
                            new ParameterInfo { Name = "priority", Type = "string", Description = "Priority of the task.", IsRequired = false }
                        ]
                    },
                    new ServiceMethodInfo
                    {
                        MethodName = "UpdateTask",
                        Description = "Update task by id, status, description, priority, and assignedTo.",
                        Parameters =
                        [
                            new ParameterInfo { Name = "id", Type = "int", Description = "Id of task to be updated.", IsRequired = true },
                            new ParameterInfo { Name = "organizationName", Type = "string", Description = "Name of organization where the task is located.", IsRequired = true },
                            new ParameterInfo { Name = "projectName", Type = "string", Description = "Name of project where the task is located.", IsRequired = true },
                            new ParameterInfo { Name = "assignedTo", Type = "string", Description = "Email of user to be assigned.", IsRequired = false },
                            new ParameterInfo { Name = "status", Type = "string", Description = "New status of the task.", IsRequired = false },
                            new ParameterInfo { Name = "description", Type = "string", Description = "New description of the task.", IsRequired = false },
                            new ParameterInfo { Name = "priority", Type = "string", Description = "New priority of the task.", IsRequired = false }
                        ]
                    },
                    new ServiceMethodInfo
                    {
                        MethodName = "DeleteTask",
                        Description = "Deletes task by id.",
                        Parameters =
                        [
                            new ParameterInfo { Name = "taskId", Type = "int", Description = "Id of task to be deleted.", IsRequired = true },
                            new ParameterInfo { Name = "organizationName", Type = "string", Description = "Name of organization where the task is located.", IsRequired = true },
                            new ParameterInfo { Name = "projectName", Type = "string", Description = "Name of project where the task is located.", IsRequired = true }
                        ]
                    },
                    new ServiceMethodInfo
                    {
                        MethodName = "GetOrganizations",
                        Description = "Retrieves a list of Azure DevOps organizations.",
                        Parameters = []
                    },
                    new ServiceMethodInfo
                    {
                        MethodName = "GetProjects",
                        Description = "Retrieves a list of projects in the specified organization.",
                        Parameters =
                        [
                            new ParameterInfo { Name = "organizationName", Type = "string", Description = "Name of the organization.", IsRequired = true }
                        ]
                    }
                ]
            };
        }
    }
}