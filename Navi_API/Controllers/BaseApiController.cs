using Microsoft.AspNetCore.Mvc;
using Shared.Responses;

namespace Navi_API.Controllers;

/// <summary>
/// Base Controller với các helper methods cho API responses
/// Tất cả controllers nên kế thừa từ class này
/// </summary>
[ApiController]
[Route("api/[controller]")]
public abstract class BaseApiController : ControllerBase
{
    /// <summary>
    /// Trả về success response với data
    /// </summary>
    protected IActionResult Success<T>(T data, string? message = null)
    {
        return Ok(ApiResponse<T>.Ok(data, message));
    }

    /// <summary>
    /// Trả về success response không có data
    /// </summary>
    protected IActionResult Success(string? message = null)
    {
        return Ok(ApiResponse.Ok(message));
    }

    /// <summary>
    /// Trả về created response (201)
    /// </summary>
    protected IActionResult Created<T>(T data, string? actionName = null, object? routeValues = null, string? message = null)
    {
        var response = ApiResponse<T>.Created(data, message);
        
        if (actionName != null)
            return CreatedAtAction(actionName, routeValues, response);
        
        return StatusCode(201, response);
    }

    /// <summary>
    /// Trả về not found response (404)
    /// </summary>
    protected IActionResult NotFoundResponse(string message)
    {
        return NotFound(ApiResponse.NotFound(message));
    }

    /// <summary>
    /// Trả về not found response với generic type
    /// </summary>
    protected IActionResult NotFoundResponse<T>(string message)
    {
        return NotFound(ApiResponse<T>.NotFound(message));
    }

    /// <summary>
    /// Trả về bad request response (400)
    /// </summary>
    protected IActionResult BadRequestResponse(string message)
    {
        return BadRequest(ApiResponse.Fail(message));
    }

    /// <summary>
    /// Trả về bad request response với generic type
    /// </summary>
    protected IActionResult BadRequestResponse<T>(string message)
    {
        return BadRequest(ApiResponse<T>.Fail(message));
    }

    /// <summary>
    /// Trả về error response với custom status code
    /// </summary>
    protected IActionResult ErrorResponse(string message, int statusCode = 500)
    {
        return StatusCode(statusCode, ApiResponse.Fail(message, statusCode));
    }
}
