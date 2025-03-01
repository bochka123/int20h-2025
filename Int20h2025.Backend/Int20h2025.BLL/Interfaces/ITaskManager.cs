using Int20h2025.Common.Models.Ai;

namespace Int20h2025.BLL.Interfaces
{
    public interface ITaskManager
    {
        public string SystemName { get; init; }
        Task<OperationResult> ExecuteMethodAsync(string methodName, object[] parameters);
        SystemMethodInfo GetAvailableMethods();
    }
}