using Microsoft.EntityFrameworkCore;
using spliteasy.Persistence.Common;
using spliteasy.Persistence.Models;

namespace spliteasy.Persistence;

public class GroupRepository(AppDbContext context) : IGroupRepository
{
    public async Task<Group> CreateGroup(string groupName, List<Guid> userIds)
    {
        var groupExists = await context.Groups.AnyAsync(x => x.Name == groupName);
        if (groupExists)
        {
            throw new GroupExists($"Group '{groupName}' already exists");
        }

        var users = await context.Users.Where(u => userIds.Contains(u.Id)).ToListAsync();

        if (users.Count != userIds.Count)
        {
            var foundUserIds = users.Select(u => u.Id).ToHashSet();
            var missingUserIds = userIds.Where(id => !foundUserIds.Contains(id)).ToList();
            throw new UserNotFound($"Users with IDs {string.Join(", ", missingUserIds)} not found");
        }

        var group = new Group
        {
            Id = Guid.NewGuid(),
            Name = groupName,
            CreatedAt = DateTime.UtcNow,
        };

        foreach (var user in users)
        {
            group.GroupMembers.Add(new GroupMember { UserId = user.Id, GroupId = group.Id });
        }

        await context.Groups.AddAsync(group);
        await context.SaveChangesAsync();

        return group;
    }
}
