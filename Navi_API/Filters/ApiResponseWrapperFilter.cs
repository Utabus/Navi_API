using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Shared.Responses;

namespace Navi_API.Filters;

/// <summary>
/// Action Filter tự động wrap response thành ApiResponse
/// </summary>
public class ApiResponseWrapperFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        // Không cần xử lý trước khi action chạy
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        // Bỏ qua nếu đã có exception hoặc response đã được set
        if (context.Exception != null || context.Result == null)
            return;

        // Bỏ qua nếu response đã là ApiResponse
        if (context.Result is ObjectResult objectResult && 
            objectResult.Value is ApiResponseBase)
            return;

        // Wrap response dựa trên HTTP status code
        switch (context.Result)
        {
            case OkObjectResult okResult:
                context.Result = new OkObjectResult(
                    ApiResponse<object>.Ok(okResult.Value!, "Thành công"));
                break;

            case CreatedAtActionResult createdResult:
                context.Result = new CreatedAtActionResult(
                    createdResult.ActionName,
                    createdResult.ControllerName,
                    createdResult.RouteValues,
                    ApiResponse<object>.Created(createdResult.Value!, "Tạo thành công"));
                break;

            case NotFoundObjectResult notFoundResult:
                context.Result = new NotFoundObjectResult(
                    ApiResponse.NotFound(notFoundResult.Value?.ToString() ?? "Không tìm thấy"));
                break;

            case BadRequestObjectResult badRequestResult:
                context.Result = new BadRequestObjectResult(
                    ApiResponse.Fail(badRequestResult.Value?.ToString() ?? "Yêu cầu không hợp lệ"));
                break;

            case NoContentResult:
                context.Result = new OkObjectResult(ApiResponse.Ok("Thành công"));
                break;
        }
    }
}
