namespace spliteasy.Persistence.Models;

public partial class ExpenseSplit
{
    public Guid ExpenseId { get; set; }

    public Guid UserId { get; set; }

    public decimal Amount { get; set; }

    public decimal? AmountExtra { get; set; }

    public virtual ExpenseEntity Expense { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
