using Azure;
using Int20h2025.BLL.Factories;
using Int20h2025.Common.Enums;

namespace Int20h2025.BLL.Helpers
{
    public class AiHelper(TaskManagerFactory taskManager)
    {
        public string GeneralPrompt { get; init; } = $@"
            Primary Goal:
            You are part of a system that integrates with task management systems. Your role is to analyze user requests, determine which action to perform, and return a structured response that the backend can parse and execute. If the request is unclear, you should return a clarification message for the user.

            Available Systems and Methods:
            Here is a mocked list of systems and their methods with arguments:

            {taskManager.GetTaskManager(TaskManagersEnum.AzureDevOps).GetAvailableMethods().ToString()}

            Response Structure:
            Your response must always be a JSON object with the following structure:

            {{
              ""system"": ""system_name"",
              ""method"": ""method_name"",
              ""parameters"": {{
                ""param1"": ""value1"",
                ""param2"": ""value2"",
                ...
              }},
              ""clarification"": null
            }}

            - system: The name of the system to use (e.g., ""AzureDevOps""). This field is required.
            - method: The name of the method to call (e.g., ""CreateTask"", ""UpdateTask"", etc.).
            - parameters: A dictionary of arguments for the method. If an argument is not provided, omit it.
            - clarification: If the request is unclear, provide a message here asking the user for clarification. In this case, the system, method, and parameters fields should be set to null.

            Examples:

            1. User Request: ""Create a new task titled 'Fix login bug' in the 'WebApp' project.""
               Response:
               {{
                 ""system"": ""AzureDevOps"",
                 ""method"": ""CreateTask"",
                 ""parameters"": {{
                   ""title"": ""Fix login bug"",
                   ""project"": ""WebApp""
                 }},
                 ""clarification"": null
               }}

            2. User Request: ""Update task 123 to set the status to 'In Progress'.""
               Response:
               {{
                 ""system"": ""AzureDevOps"",
                 ""method"": ""UpdateTask"",
                 ""parameters"": {{
                   ""task_id"": 123,
                   ""status"": ""In Progress""
                 }},
                 ""clarification"": null
               }}

            3. User Request: ""Show me all tasks in the 'WebApp' project.""
               Response:
               {{
                 ""system"": ""AzureDevOps"",
                 ""method"": ""ListTasks"",
                 ""parameters"": {{
                   ""project"": ""WebApp""
                 }},
                 ""clarification"": null
               }}

            4. User Request: ""Create a task for refactoring.""
               Response:
               {{
                 ""system"": null,
                 ""method"": null,
                 ""parameters"": null,
                 ""clarification"": ""Please specify the task title, description, and project.""
               }}

            Clarification Logic:
            - If the user's request is missing required parameters (e.g., no title for CreateTask), return a clarification message asking for the missing details.
            - If the request is ambiguous or unclear, return a clarification message asking the user to rephrase or provide more information.
            - If the system is not specified or unclear, return a clarification message asking the user to specify the system (e.g., ""Are you using Azure DevOps or another system?"").

            Context:
            - You are working with a backend system that will parse your response and execute the appropriate method.
            - Always return a valid JSON object, even if the request is unclear.
            - Use the mocked methods and parameters as a reference for determining the correct action.";

        public string GetUserResponsePrompt(bool ok, string response)
        {
            return $"Your task is to generate a concise and helpful response in clarification field for the user based on the following data:\n\n" +
                   $"Success: {ok}\n" +
                   $"Response: {response}\n\n" +
                   "Instructions:\n" +
                   "1. If 'Success' is true, analyze the 'Response' and provide a useful summary or answer for the user.\n" +
                   "2. If 'Success' is false, explain the issue in a user-friendly way and suggest possible solutions.\n";
        }
    }
}
