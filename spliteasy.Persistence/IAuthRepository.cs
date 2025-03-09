using spliteasy.Persistence.Models;

namespace spliteasy.Persistence;

public interface IAuthRepository
{
    public Task<User?> GetUserById(Guid userId);
    public Task<User> CreateUser(string username, string password);
    public Task<User> GetUser(string username, string password);
}
