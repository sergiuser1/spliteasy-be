using System;
using System.Collections.Generic;

namespace spliteasy.Persistence.Models;

public partial class ExpenseType
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Expense> Expenses { get; set; } = new List<Expense>();
}
