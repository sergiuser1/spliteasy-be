using spliteasy.Persistence.Models;

namespace spliteasy.Persistence;

public interface IGroupRepository
{
    public Task<Group> CreateGroup(string groupName, List<Guid> userIds);
    public Task<Group> AddUsersToGroup(Guid groupId, List<Guid> userIds);
}
