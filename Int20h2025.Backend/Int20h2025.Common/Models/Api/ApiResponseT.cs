namespace Int20h2025.Common.Models.Api
{
    public class ApiResponse<T>: ApiResponse
    {
        public ApiResponse(bool success) : base(success)
        {
        }

        public ApiResponse(bool success, string? message) : base(success, message)
        {
        }

        public ApiResponse(T? data): base(true)
        {
            Data = data;
        }

        public T? Data { get; set; }
    }
}
