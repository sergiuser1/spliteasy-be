using spliteasy.Persistence.Models;

namespace spliteasy.Persistence;

public interface IGroupRepository
{
    public Task<Group> CreateGroup(string groupName, List<Guid> userIds);
}
