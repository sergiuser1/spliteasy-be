using System.Text.Json.Serialization;

namespace spliteasy.Persistence.Models;

public partial class ExpenseType
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public ExpenseTypeEnum Type =>
        Name.ToLowerInvariant() switch
        {
            "equal" => ExpenseTypeEnum.Equal,
            "exact" => ExpenseTypeEnum.Exact,
            "percentage" => ExpenseTypeEnum.Percentage,
            "adjustment" => ExpenseTypeEnum.Adjustment,
            _ => throw new ArgumentOutOfRangeException(nameof(Name)),
        };
    public string? Description { get; set; }

    public virtual ICollection<ExpenseEntity> Expenses { get; set; } = new List<ExpenseEntity>();
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ExpenseTypeEnum
{
    Equal,
    Exact,
    Percentage,
    Adjustment,
}

public static class ExpenseTypeExtensions
{
    public static string ToNameString(this ExpenseTypeEnum type) =>
        type switch
        {
            ExpenseTypeEnum.Equal => "equal",
            ExpenseTypeEnum.Exact => "exact",
            ExpenseTypeEnum.Percentage => "percentage",
            ExpenseTypeEnum.Adjustment => "adjustment",
            _ => throw new ArgumentOutOfRangeException(nameof(type)),
        };
}
