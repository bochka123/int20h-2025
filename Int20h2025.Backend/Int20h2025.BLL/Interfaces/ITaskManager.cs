using Int20h2025.Common.Models.Ai;
using Newtonsoft.Json.Linq;

namespace Int20h2025.BLL.Interfaces
{
    public interface ITaskManager
    {
        public DAL.Entities.System System { get; }
        Task<OperationResult> ExecuteMethodAsync(string methodName, JObject parameters);
        SystemMethodInfo GetAvailableMethods();
    }
}