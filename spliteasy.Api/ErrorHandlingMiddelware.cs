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
            UserExists => CreateErrorResponse(StatusCodes.Status409Conflict, "User already exists"),

            WrongPassword => CreateErrorResponse(
                StatusCodes.Status401Unauthorized,
                "Wrong password"
            ),

            _ => null,
        };

        if (errorResponse is null)
        {
            _logger.LogError(exception, "An unhandled exception occurred.");
            throw exception;
        }

        response.StatusCode = errorResponse.StatusCode;
        await response.WriteAsync(JsonSerializer.Serialize(errorResponse));
    }

    private ErrorResponse CreateErrorResponse(int statusCode, string message)
    {
        return new ErrorResponse { StatusCode = statusCode, Message = message };
    }
}
