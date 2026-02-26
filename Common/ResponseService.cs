namespace PharmacyManagementSystem.Common;

public class ResponseService
{
    public ApiResponse<T> SuccessResponse<T>(T data, string message)
    {
        return new ApiResponse<T>
        {
            Success = true,
            Message = message,
            Data = data
        };
    }

    public ApiResponse<T> FailResponse<T>(string message)
    {
        return new ApiResponse<T>
        {
            Success = false,
            Message = message,
            Data = default
        };
    }

    internal object? FailResponse<T>(object message)
    {
        throw new NotImplementedException();
    }

    internal object? SuccessResponse(object data, string message)
    {
        throw new NotImplementedException();
    }
}