using spliteasy.Persistence.Models;

namespace SplitEasy.Models
{
    // Auth DTOs
    public class MeResponse
    {
        public required string Username { get; set; }
        public Guid UserId { get; set; }
    }

    public class SignUpRequest
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }

    public class SignUpResponse
    {
        public required string Username { get; set; }
        public required Guid UserId { get; set; }
        public required string Token { get; set; }
    }

    public class SignInRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class SignInResponse
    {
        public Guid UserId { get; set; }
        public string Token { get; set; }
    }

    // Group DTOs
    public class CreateGroupRequest
    {
        public required string Name { get; set; }
        public required List<Guid> UserIds { get; set; }
    }

    public class CreateGroupResponse
    {
        public Guid GroupId { get; set; }
    }

    public class AddUsersRequest
    {
        public required List<Guid> UserIds { get; set; }
    }

    public class DeleteUsersRequest
    {
        public required List<Guid> UserIds { get; set; }
    }

    public class ExpenseSplitDetail
    {
        public Guid UserId { get; set; }
        public decimal Amount { get; set; }
    }

    public class Expense
    {
        public required string Title { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public Guid PaidByUserId { get; set; }
        public required ExpenseTypeEnum Split { get; set; }
    }

    public class GetExpensesResponse
    {
        public List<Expense> Expenses { get; set; }
    }

    public class UserBalance
    {
        public Guid UserId { get; set; }
        public decimal Amount { get; set; }
    }

    public class UserBalanceItem
    {
        public Guid UserId { get; set; }
        public List<UserBalance> Balance { get; set; }
    }

    public class GetBalancesResponse
    {
        public List<UserBalanceItem> UserBalances { get; set; }
    }

    public class SettleRequest
    {
        public Guid PayerUserId { get; set; }
        public Guid ReceiverUserId { get; set; }
        public decimal Amount { get; set; }
    }
}
