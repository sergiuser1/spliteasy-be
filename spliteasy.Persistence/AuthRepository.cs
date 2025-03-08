using Microsoft.EntityFrameworkCore;
using spliteasy.Persistence.Common;
using spliteasy.Persistence.Models;

namespace spliteasy.Persistence;

public class AuthRepository : IAuthRepository
{
    private readonly AppDbContext _context;

    public AuthRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetUserById(Guid userId)
    {
        return await _context.Users.FindAsync(userId);
    }

    public async Task<User> CreateUser(string username, string password)
    {
        if (await _context.Users.AnyAsync(x => x.Username == username))
        {
            throw new UserExists($"User {username} already exists");
        }

        Guid userId = Guid.NewGuid();

        var user = new User
        {
            Username = username,
            Id = userId,

            PasswordHash = password, // TODO: Not good
        };

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        return user;
    }
}
