using System;
using System.Collections.Generic;

namespace spliteasy.Persistence.Models;

public partial class Expense
{
    public Guid Id { get; set; }

    public Guid GroupId { get; set; }

    public Guid UserId { get; set; }

    public Guid ExpenseTypeId { get; set; }

    public decimal Amount { get; set; }

    public string? Description { get; set; }

    public DateTime DateIncurred { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<ExpenseSplit> ExpenseSplits { get; set; } = new List<ExpenseSplit>();

    public virtual ExpenseType ExpenseType { get; set; } = null!;

    public virtual Group Group { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
