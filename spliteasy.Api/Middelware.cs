using System.Text.Json;
using spliteasy.Persistence.Common;

namespace spliteasy.Api;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {

        var response = context.Response;
        response.ContentType = "application/json";

        var errorResponse = exception switch
        {
            NotFoundException => CreateErrorResponse(
                StatusCodes.Status404NotFound,
                "Resource not found",
                exception
            ),
            ValidationException => CreateErrorResponse(
                StatusCodes.Status400BadRequest,
                "Validation failed",
                exception
            ),
            UnauthorizedException => CreateErrorResponse(
                StatusCodes.Status401Unauthorized,
                "Unauthorized access",
                exception
            ),
            ForbiddenException => CreateErrorResponse(
                StatusCodes.Status403Forbidden,
                "Forbidden",
                exception
            ),
            //
            // Custom errors
            UserExists => CreateErrorResponse(
                StatusCodes.Status409Conflict,
                "User already exists",
                null
            ),

            _ => CreateErrorResponse(
                StatusCodes.Status500InternalServerError,
                "An unexpected error occurred",
                exception
            ),
        };

        // TODO: Do this properly, split into handled and not handled errors
        if (errorResponse.StatusCode == StatusCodes.Status500InternalServerError)
        {
            _logger.LogError(exception, "An unhandled exception occurred.");
        }

        response.StatusCode = errorResponse.StatusCode;
        await response.WriteAsync(JsonSerializer.Serialize(errorResponse));
    }

    private ErrorResponse CreateErrorResponse(int statusCode, string message, Exception? exception)
    {
        return new ErrorResponse
        {
            StatusCode = statusCode,
            Message = message,
            DetailedMessage = exception?.Message ?? "",
            StackTrace = exception?.StackTrace ?? "",
        };
    }
}
