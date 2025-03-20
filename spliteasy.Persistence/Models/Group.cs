namespace spliteasy.Persistence.Models;

public partial class Group
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<ExpenseEntity> Expenses { get; set; } = new List<ExpenseEntity>();

    public virtual ICollection<GroupMember> GroupMembers { get; set; } = new List<GroupMember>();
}
