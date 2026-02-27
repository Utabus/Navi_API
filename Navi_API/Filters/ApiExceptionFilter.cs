using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Shared.Responses;
using System.Net;

namespace Navi_API.Filters;

/// <summary>
/// Exception Filter xử lý exceptions và trả về ApiResponse format
/// </summary>
public class ApiExceptionFilter : IExceptionFilter
{
    private readonly ILogger<ApiExceptionFilter> _logger;

    public ApiExceptionFilter(ILogger<ApiExceptionFilter> logger)
    {
        _logger = logger;
    }

    public void OnException(ExceptionContext context)
    {
        var statusCode = HttpStatusCode.InternalServerError;
        var message = "Đã xảy ra lỗi không mong muốn";

        // Xử lý các loại exception cụ thể
        switch (context.Exception)
        {
            case KeyNotFoundException:
                statusCode = HttpStatusCode.NotFound;
                message = context.Exception.Message;
                break;

            case ArgumentException:
            case InvalidOperationException:
                statusCode = HttpStatusCode.BadRequest;
                message = context.Exception.Message;
                break;

            case UnauthorizedAccessException:
                statusCode = HttpStatusCode.Unauthorized;
                message = "Không có quyền truy cập";
                break;

            default:
                _logger.LogError(context.Exception, "Lỗi không xử lý được: {Message}", context.Exception.Message);
                break;
        }

        var response = ApiResponse.Fail(message, (int)statusCode);

        context.Result = new ObjectResult(response)
        {
            StatusCode = (int)statusCode
        };

        context.ExceptionHandled = true;
    }
}
