namespace Inventory.Shared;

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public List<string>? Errors { get; set; }
    public int StatusCode { get; set; }

    public ApiResponse()
    {
    }

    public ApiResponse(T data, string message = "", int statusCode = 200)
    {
        Success = true;
        Message = message;
        Data = data;
        StatusCode = statusCode;
    }

    public ApiResponse(int statusCode, string message, List<string>? errors = null)
    {
        Success = false;
        Message = message;
        Errors = errors;
        StatusCode = statusCode;
    }
}
