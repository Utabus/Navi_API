namespace Shared.Responses;

/// <summary>
/// Base API Response không có data
/// </summary>
public class ApiResponseBase
{
    public bool Success { get; set; }
    public int StatusCode { get; set; }
    public string? Message { get; set; }
    public IEnumerable<string>? Errors { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}

/// <summary>
/// API Response với data - kế thừa từ ApiResponseBase
/// </summary>
/// <typeparam name="T">Kiểu dữ liệu trả về</typeparam>
public class ApiResponse<T> : ApiResponseBase
{
    public T? Data { get; set; }

    /// <summary>
    /// Tạo response thành công
    /// </summary>
    public static ApiResponse<T> Ok(T data, string? message = null)
    {
        return new ApiResponse<T>
        {
            Success = true,
            StatusCode = 200,
            Message = message ?? "Thành công",
            Data = data
        };
    }

    /// <summary>
    /// Tạo response Created (201)
    /// </summary>
    public static ApiResponse<T> Created(T data, string? message = null)
    {
        return new ApiResponse<T>
        {
            Success = true,
            StatusCode = 201,
            Message = message ?? "Tạo thành công",
            Data = data
        };
    }

    /// <summary>
    /// Tạo response thất bại
    /// </summary>
    public static ApiResponse<T> Fail(string message, int statusCode = 400, IEnumerable<string>? errors = null)
    {
        return new ApiResponse<T>
        {
            Success = false,
            StatusCode = statusCode,
            Message = message,
            Errors = errors
        };
    }

    /// <summary>
    /// Tạo response Not Found
    /// </summary>
    public static ApiResponse<T> NotFound(string message)
    {
        return new ApiResponse<T>
        {
            Success = false,
            StatusCode = 404,
            Message = message
        };
    }
}

/// <summary>
/// API Response không có data - kế thừa từ ApiResponseBase
/// Dùng cho các operations không trả về data (Update, Delete)
/// </summary>
public class ApiResponse : ApiResponseBase
{
    public static ApiResponse Ok(string? message = null)
    {
        return new ApiResponse
        {
            Success = true,
            StatusCode = 200,
            Message = message ?? "Thành công"
        };
    }

    public static ApiResponse Fail(string message, int statusCode = 400, IEnumerable<string>? errors = null)
    {
        return new ApiResponse
        {
            Success = false,
            StatusCode = statusCode,
            Message = message,
            Errors = errors
        };
    }

    public static ApiResponse NotFound(string message)
    {
        return new ApiResponse
        {
            Success = false,
            StatusCode = 404,
            Message = message
        };
    }
}
