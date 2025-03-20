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
            throw new AlreadyExists($"Group '{groupName}' already exists");
        }

        var users = await context.Users.Where(u => userIds.Contains(u.Id)).ToListAsync();

        if (users.Count != userIds.Count)
        {
            var foundUserIds = users.Select(u => u.Id).ToHashSet();
            var missingUserIds = userIds.Where(id => !foundUserIds.Contains(id)).ToList();
            throw new NotFound($"Users with IDs {string.Join(", ", missingUserIds)} not found");
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

    public async Task<Group> AddUsersToGroup(Guid groupId, List<Guid> userIds)
    {
        var group = await context
            .Groups.Include(x => x.GroupMembers)
            .FirstOrDefaultAsync(x => x.Id == groupId);

        if (group is null)
        {
            throw new NotFound($"Group with ID '{groupId}' does not exist");
        }

        if (group.GroupMembers.Any(x => userIds.Contains(x.UserId)))
        {
            var groupMembersIds = group.GroupMembers.Select(u => u.UserId).ToHashSet();
            var alreadyExistingMembers = userIds.Where(id => groupMembersIds.Contains(id)).ToList();

            throw new AlreadyExists(
                $"Group with ID '{groupId}' already contains users '{string.Join(", ", alreadyExistingMembers)}"
            );
        }

        var users = await context.Users.Where(u => userIds.Contains(u.Id)).ToListAsync();

        if (users.Count != userIds.Count)
        {
            var foundUserIds = users.Select(u => u.Id).ToHashSet();
            var missingUserIds = userIds.Where(id => !foundUserIds.Contains(id)).ToList();
            throw new NotFound($"Users with IDs {string.Join(", ", missingUserIds)} not found");
        }

        foreach (var user in users)
        {
            group.GroupMembers.Add(new GroupMember { UserId = user.Id, GroupId = group.Id });
        }

        context.Groups.Update(group);
        await context.SaveChangesAsync();

        return group;
    }

    public async Task AddExpenseToGroup(ExpenseEntity expense)
    {
        var group = await context
            .Groups.Include(x => x.GroupMembers)
            .FirstOrDefaultAsync(x => x.Id == expense.GroupId);

        if (group is null)
        {
            throw new NotFound($"Group with ID '{expense.GroupId}' does not exist");
        }

        if (!group.GroupMembers.Any(x => x.UserId == expense.UserId))
        {
            throw new NotFound(
                $"Group with ID '{expense.GroupId}' does not containe user '{expense.UserId}'"
            );
        }
    }
}
