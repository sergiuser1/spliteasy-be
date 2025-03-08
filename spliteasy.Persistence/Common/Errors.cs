namespace spliteasy.Persistence.Common;

public class ErrorResponse
{
    public int StatusCode { get; set; }
    public required string Message { get; set; }
    public required string DetailedMessage { get; set; }
    public required string StackTrace { get; set; }
}

public class NotFoundException : Exception
{
    public NotFoundException(string message)
        : base(message) { }
}

public class ValidationException : Exception
{
    public ValidationException(string message)
        : base(message) { }
}

public class UnauthorizedException : Exception
{
    public UnauthorizedException(string message)
        : base(message) { }
}

public class ForbiddenException : Exception
{
    public ForbiddenException(string message)
        : base(message) { }
}

public class UserExists(string message) : Exception(message) { }
