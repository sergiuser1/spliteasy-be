using Microsoft.EntityFrameworkCore;
using spliteasy.Persistence.Models;

namespace spliteasy.Persistence;

public partial class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public virtual required DbSet<Expense> Expenses { get; set; }

    public virtual required DbSet<ExpenseSplit> ExpenseSplits { get; set; }

    public virtual required DbSet<ExpenseType> ExpenseTypes { get; set; }

    public virtual required DbSet<FlywaySchemaHistory> FlywaySchemaHistories { get; set; }

    public virtual required DbSet<Group> Groups { get; set; }

    public virtual required DbSet<GroupMember> GroupMembers { get; set; }

    public virtual required DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Expense>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("expenses_pkey");

            entity.ToTable("expenses");

            entity.HasIndex(e => e.GroupId, "idx_expenses_group_id");

            entity.HasIndex(e => e.UserId, "idx_expenses_user_id");

            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()").HasColumnName("id");
            entity.Property(e => e.Amount).HasPrecision(10, 2).HasColumnName("amount");
            entity
                .Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity
                .Property(e => e.DateIncurred)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("date_incurred");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.ExpenseTypeId).HasColumnName("expense_type_id");
            entity.Property(e => e.GroupId).HasColumnName("group_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity
                .HasOne(d => d.ExpenseType)
                .WithMany(p => p.Expenses)
                .HasForeignKey(d => d.ExpenseTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("expenses_expense_type_id_fkey");

            entity
                .HasOne(d => d.Group)
                .WithMany(p => p.Expenses)
                .HasForeignKey(d => d.GroupId)
                .HasConstraintName("expenses_group_id_fkey");

            entity
                .HasOne(d => d.User)
                .WithMany(p => p.Expenses)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("expenses_user_id_fkey");
        });

        modelBuilder.Entity<ExpenseSplit>(entity =>
        {
            entity.HasKey(e => new { e.ExpenseId, e.UserId }).HasName("expense_splits_pkey");

            entity.ToTable("expense_splits");

            entity.HasIndex(e => e.ExpenseId, "idx_expense_splits_expense_id");

            entity.HasIndex(e => e.UserId, "idx_expense_splits_user_id");

            entity.Property(e => e.ExpenseId).HasColumnName("expense_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Amount).HasPrecision(10, 2).HasColumnName("amount");
            entity.Property(e => e.AmountExtra).HasPrecision(10, 2).HasColumnName("amount_extra");

            entity
                .HasOne(d => d.Expense)
                .WithMany(p => p.ExpenseSplits)
                .HasForeignKey(d => d.ExpenseId)
                .HasConstraintName("expense_splits_expense_id_fkey");

            entity
                .HasOne(d => d.User)
                .WithMany(p => p.ExpenseSplits)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("expense_splits_user_id_fkey");
        });

        modelBuilder.Entity<ExpenseType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("expense_types_pkey");

            entity.ToTable("expense_types");

            entity.HasIndex(e => e.Name, "expense_types_name_key").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()").HasColumnName("id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Name).HasMaxLength(50).HasColumnName("name");
        });

        modelBuilder.Entity<FlywaySchemaHistory>(entity =>
        {
            entity.HasKey(e => e.InstalledRank).HasName("flyway_schema_history_pk");

            entity.ToTable("flyway_schema_history");

            entity.HasIndex(e => e.Success, "flyway_schema_history_s_idx");

            entity
                .Property(e => e.InstalledRank)
                .ValueGeneratedNever()
                .HasColumnName("installed_rank");
            entity.Property(e => e.Checksum).HasColumnName("checksum");
            entity.Property(e => e.Description).HasMaxLength(200).HasColumnName("description");
            entity.Property(e => e.ExecutionTime).HasColumnName("execution_time");
            entity.Property(e => e.InstalledBy).HasMaxLength(100).HasColumnName("installed_by");
            entity
                .Property(e => e.InstalledOn)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("installed_on");
            entity.Property(e => e.Script).HasMaxLength(1000).HasColumnName("script");
            entity.Property(e => e.Success).HasColumnName("success");
            entity.Property(e => e.Type).HasMaxLength(20).HasColumnName("type");
            entity.Property(e => e.Version).HasMaxLength(50).HasColumnName("version");
        });

        modelBuilder.Entity<Group>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("groups_pkey");

            entity.ToTable("groups");

            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()").HasColumnName("id");
            entity
                .Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Name).HasMaxLength(100).HasColumnName("name");
        });

        modelBuilder.Entity<GroupMember>(entity =>
        {
            entity.HasKey(e => new { e.GroupId, e.UserId }).HasName("group_members_pkey");

            entity.ToTable("group_members");

            entity.HasIndex(e => e.GroupId, "idx_group_members_group_id");

            entity.HasIndex(e => e.UserId, "idx_group_members_user_id");

            entity.Property(e => e.GroupId).HasColumnName("group_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity
                .Property(e => e.JoinedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("joined_at");

            entity
                .HasOne(d => d.Group)
                .WithMany(p => p.GroupMembers)
                .HasForeignKey(d => d.GroupId)
                .HasConstraintName("group_members_group_id_fkey");

            entity
                .HasOne(d => d.User)
                .WithMany(p => p.GroupMembers)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("group_members_user_id_fkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pkey");

            entity.ToTable("users");

            entity.HasIndex(e => e.Username, "users_username_key").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever().HasColumnName("id");
            entity
                .Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.PasswordHash).HasMaxLength(255).HasColumnName("password_hash");
            entity
                .Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");
            entity.Property(e => e.Username).HasMaxLength(50).HasColumnName("username");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
