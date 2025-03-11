namespace spliteasy.Persistence.Common;

public class ErrorResponse
{
    public int StatusCode { get; set; }
    public required string Message { get; set; }
}

public class UserExists(string message) : Exception(message) { }

public class UserNotFound(string message) : Exception(message) { }

public class WrongPassword(string message) : Exception(message) { }

public class GroupExists(string message) : Exception(message) { }
