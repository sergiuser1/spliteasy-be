namespace spliteasy.Persistence.Models;

public partial class User
{
    public Guid Id { get; set; }

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<ExpenseSplit> ExpenseSplits { get; set; } = new List<ExpenseSplit>();

    public virtual ICollection<ExpenseEntity> Expenses { get; set; } = new List<ExpenseEntity>();

    public virtual ICollection<GroupMember> GroupMembers { get; set; } = new List<GroupMember>();
}
