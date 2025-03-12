namespace spliteasy.Persistence.Common;

public class ErrorResponse
{
    public int StatusCode { get; set; }
    public required string Message { get; set; }
}

public class AlreadyExists(string message) : Exception(message) { }

public class NotFound(string message) : Exception(message) { }

public class WrongPassword(string message) : Exception(message) { }
