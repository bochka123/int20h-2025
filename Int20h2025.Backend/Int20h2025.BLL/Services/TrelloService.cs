using Int20h2025.Auth.Interfaces;
using Int20h2025.BLL.Interfaces;
using Int20h2025.Common.Enums;
using Int20h2025.Common.Exceptions;
using Int20h2025.Common.Models.Ai;
using Int20h2025.DAL.Context;
using Newtonsoft.Json.Linq;
using TrelloDotNet;
using TrelloDotNet.Model;

namespace Int20h2025.BLL.Services
{
    public class TrelloService(Int20h2025Context context, ITrelloAuthService trelloAuthService, IUserContextService userContextService) : ITaskManager
    {
        private TrelloClient trelloClient;
        public DAL.Entities.System System => context.Systems.FirstOrDefault(x => x.Name == nameof(TaskManagersEnum.Trello))
                                            ?? throw new InternalPointerBobrException("System not configured");

        public async Task<OperationResult> ExecuteMethodAsync(string methodName, JObject parameters)
        {
            var integration = context.Integrations.FirstOrDefault(x => x.SystemId == System.Id && x.ProfileId == userContextService.UserId);

            if (integration == null) return new OperationResult { Response = $"User doesn't integrated with '{nameof(TaskManagersEnum.Trello)}'. Provide ApiKey and Token to integrate.", Success = false };

            trelloClient = new TrelloClient(integration.ApiKey, integration.Token);
            switch (methodName)
            {
                case "CreateTask":
                    var title = parameters["title"].ToString();
                    var description = parameters["description"].ToString();
                    var listId = parameters["listId"]?.ToString();
                    var listName = parameters["listName"]?.ToString();
                    var boardId = parameters["boardId"]?.ToString();
                    var boardName = parameters["boardName"]?.ToString();
                    return await CreateTaskAsync(title, description, listId, listName, boardId, boardName);

                case "UpdateTask":
                    var cardId = parameters["taskId"]?.ToString();
                    var newTitle = parameters["title"].ToString();
                    var taskName = parameters["taskName"]?.ToString();
                    var updateBoardId = parameters["boardId"]?.ToString();
                    var updateBoardName = parameters["boardName"]?.ToString();
                    return await UpdateTaskAsync(cardId, newTitle, taskName, updateBoardId, updateBoardName);

                case "DeleteTask":
                    var deleteCardId = parameters["taskId"]?.ToString();
                    var deleteTaskName = parameters["taskName"]?.ToString();
                    var deleteBoardId = parameters["boardId"]?.ToString();
                    var deleteBoardName = parameters["boardName"]?.ToString();
                    return await DeleteTaskAsync(deleteCardId, deleteTaskName, deleteBoardId, deleteBoardName);

                case "GetTasks":
                    var getTasksBoardId = parameters["boardId"]?.ToString();
                    var getTasksBoardName = parameters["boardName"]?.ToString();
                    return await GetTasksAsync(getTasksBoardId, getTasksBoardName);

                case "GetTask":
                    var getCardId = parameters["taskId"].ToString();
                    return await GetTaskAsync(getCardId);

                default:
                    throw new NotImplementedException($"Method '{methodName}' is not implemented.");
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
                        Description = "Creates a new card in the specified list.",
                        Parameters =
                        [
                            new ParameterInfo { Name = "title", Type = "string", Description = "Card title.", IsRequired = true },
                            new ParameterInfo { Name = "description", Type = "string", Description = "Card description.", IsRequired = true },
                            new ParameterInfo { Name = "listId", Type = "string", Description = "List ID where the card will be created.", IsRequired = false },
                            new ParameterInfo { Name = "listName", Type = "string", Description = "List name to search for if listId is not provided.", IsRequired = false },
                            new ParameterInfo { Name = "boardId", Type = "string", Description = "Board ID where the list is located.", IsRequired = false },
                            new ParameterInfo { Name = "boardName", Type = "string", Description = "Board name to search for if boardId is not provided.", IsRequired = false }
                        ]
                    },
                    new ServiceMethodInfo
                    {
                        MethodName = "UpdateTask",
                        Description = "Updates the title of a card.",
                        Parameters =
                        [
                            new ParameterInfo { Name = "taskId", Type = "string", Description = "Card ID to be updated.", IsRequired = false },
                            new ParameterInfo { Name = "title", Type = "string", Description = "New title.", IsRequired = true },
                            new ParameterInfo { Name = "taskName", Type = "string", Description = "Card name to search for if taskId is not provided.", IsRequired = false },
                            new ParameterInfo { Name = "boardId", Type = "string", Description = "Board ID where the card is located.", IsRequired = false },
                            new ParameterInfo { Name = "boardName", Type = "string", Description = "Board name to search for if boardId is not provided.", IsRequired = false }
                        ]
                    },
                    new ServiceMethodInfo
                    {
                        MethodName = "DeleteTask",
                        Description = "Deletes a card.",
                        Parameters =
                        [
                            new ParameterInfo { Name = "taskId", Type = "string", Description = "Card ID to be deleted.", IsRequired = false },
                            new ParameterInfo { Name = "taskName", Type = "string", Description = "Card name to search for if taskId is not provided.", IsRequired = false },
                            new ParameterInfo { Name = "boardId", Type = "string", Description = "Board ID where the card is located.", IsRequired = false },
                            new ParameterInfo { Name = "boardName", Type = "string", Description = "Board name to search for if boardId is not provided.", IsRequired = false }
                        ]
                    },
                    new ServiceMethodInfo
                    {
                        MethodName = "GetTasks",
                        Description = "Retrieves all cards from a board.",
                        Parameters =
                        [
                            new ParameterInfo { Name = "boardId", Type = "string", Description = "Board ID to fetch cards from.", IsRequired = false },
                            new ParameterInfo { Name = "boardName", Type = "string", Description = "Board name to search for if boardId is not provided.", IsRequired = false }
                        ]
                    },
                    new ServiceMethodInfo
                    {
                        MethodName = "GetTask",
                        Description = "Retrieves a single card by ID.",
                        Parameters =
                        [
                            new ParameterInfo { Name = "taskId", Type = "string", Description = "Card ID to fetch.", IsRequired = true }
                        ]
                    }
                ]
            };
        }

        private async Task<string?> GetBoardIdAsync(string? boardId, string? boardName)
        {
            if (!string.IsNullOrEmpty(boardId))
            {
                return boardId;
            }

            if (!string.IsNullOrEmpty(boardName))
            {
                var boards = await trelloClient.GetBoardsForMemberAsync("me");
                var board = boards.FirstOrDefault(b => b.Name.Equals(boardName, StringComparison.OrdinalIgnoreCase));
                if (board != null)
                {
                    return board.Id;
                }
            }

            return null;
        }

        private async Task<OperationResult> CreateTaskAsync(string title, string description, string? listId, string? listName, string? boardId, string? boardName)
        {
            var resolvedBoardId = await GetBoardIdAsync(boardId, boardName);
            if (resolvedBoardId == null)
            {
                return new OperationResult { Response = "Board ID or name must be provided.", Success = false };
            }

            if (string.IsNullOrEmpty(listId))
            {
                if (string.IsNullOrEmpty(listName))
                {
                    return new OperationResult { Response = "Either listId or listName must be provided.", Success = false };
                }

                var lists = await trelloClient.GetListsOnBoardAsync(resolvedBoardId);
                var list = lists.FirstOrDefault(l => l.Name.Equals(listName, StringComparison.OrdinalIgnoreCase));
                if (list == null)
                {
                    return new OperationResult { Response = $"List with name '{listName}' not found.", Success = false };
                }

                listId = list.Id;
            }

            var card = new Card(listId, title) { Description = description };
            await trelloClient.AddCardAsync(card);
            return new OperationResult { Response = $"Card '{title}' created successfully.", Success = true };
        }

        private async Task<OperationResult> UpdateTaskAsync(string? taskId, string newTitle, string? taskName = null, string? boardId = null, string? boardName = null)
        {
            if (string.IsNullOrEmpty(taskId))
            {
                if (string.IsNullOrEmpty(taskName))
                {
                    return new OperationResult { Response = "Either taskId or taskName must be provided.", Success = false };
                }

                var resolvedBoardId = await GetBoardIdAsync(boardId, boardName);
                if (resolvedBoardId == null)
                {
                    return new OperationResult { Response = "Board ID or name must be provided.", Success = false };
                }

                var cards = await trelloClient.GetCardsOnBoardAsync(resolvedBoardId);
                var card = cards.FirstOrDefault(c => c.Name.Equals(taskName, StringComparison.OrdinalIgnoreCase));
                if (card == null)
                {
                    return new OperationResult { Response = $"Card with name '{taskName}' not found.", Success = false };
                }

                taskId = card.Id;
            }

            var cardToUpdate = await trelloClient.GetCardAsync(taskId);
            cardToUpdate.Name = newTitle;
            await trelloClient.UpdateCardAsync(cardToUpdate);
            return new OperationResult { Response = $"Card '{taskId}' updated successfully.", Success = true };
        }

        private async Task<OperationResult> DeleteTaskAsync(string? taskId, string? taskName = null, string? boardId = null, string? boardName = null)
        {
            if (string.IsNullOrEmpty(taskId))
            {
                if (string.IsNullOrEmpty(taskName))
                {
                    return new OperationResult { Response = "Either taskId or taskName must be provided.", Success = false };
                }

                var resolvedBoardId = await GetBoardIdAsync(boardId, boardName);
                if (resolvedBoardId == null)
                {
                    return new OperationResult { Response = "Board ID or name must be provided.", Success = false };
                }

                var cards = await trelloClient.GetCardsOnBoardAsync(resolvedBoardId);
                var card = cards.FirstOrDefault(c => c.Name.Equals(taskName, StringComparison.OrdinalIgnoreCase));
                if (card == null)
                {
                    return new OperationResult { Response = $"Card with name '{taskName}' not found.", Success = false };
                }

                taskId = card.Id;
            }

            await trelloClient.DeleteCardAsync(taskId);
            return new OperationResult { Response = $"Card '{taskId}' deleted successfully.", Success = true };
        }

        private async Task<OperationResult> GetTasksAsync(string? boardId, string? boardName)
        {
            var resolvedBoardId = await GetBoardIdAsync(boardId, boardName);
            if (resolvedBoardId == null)
            {
                return new OperationResult { Response = "Board ID or name must be provided.", Success = false };
            }

            var cards = await trelloClient.GetCardsOnBoardAsync(resolvedBoardId);
            return new OperationResult { Response = JArray.FromObject(cards).ToString(), Success = true };
        }

        private async Task<OperationResult> GetTaskAsync(string cardId)
        {
            var card = await trelloClient.GetCardAsync(cardId);
            return new OperationResult { Response = JObject.FromObject(card).ToString(), Success = true };
        }
    }
}