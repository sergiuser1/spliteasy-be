using spliteasy.Persistence.Models;

namespace spliteasy.Auth;

public interface IAuthService
{
    public string GenerateToken(User user);
}
