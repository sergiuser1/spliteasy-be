using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SplitEasy.Models;
using spliteasy.Persistence;

namespace ExpenseSharingApp.Controllers;

[ApiController]
[Route("groups")]
[Authorize]
public class GroupsController(IGroupRepository groupRepository) : ControllerBase
{
    // In a real app, inject services like IGroupService, IUserService, etc.

    [HttpPost]
    public async Task<ActionResult<CreateGroupResponse>> CreateGroup(
        [FromBody] CreateGroupRequest groupRequest
    )
    {
        if (string.IsNullOrWhiteSpace(groupRequest.Name))
        {
            return BadRequest("Name not provided");
        }

        if (groupRequest.UserIds == null || groupRequest.UserIds.Count == 0)
        {
            return BadRequest("No user provided for the group");
        }

        // TODO: Check if all users exist


        var group = await groupRepository.CreateGroup(groupRequest.Name, groupRequest.UserIds);

        return Ok(new CreateGroupResponse { GroupId = group.Id });
    }

    [HttpPost("{groupId}/add-user")]
    public async Task<ActionResult> AddUsers(Guid groupId, [FromBody] AddUsersRequest request)
    {
        // Validate request
        if (request.UserIds == null || request.UserIds.Count == 0)
        {
            return BadRequest("No user provided for the group");
        }

        // Check if group exists
        var groupExists = true; // Replace with actual group check logic
        if (!groupExists)
        {
            return NotFound("Group not found");
        }

        // Check if all users exist
        var allUsersExist = true; // Replace with actual user check logic
        if (!allUsersExist)
        {
            return NotFound("User not found");
        }

        // Add users to group
        // Implementation would go here

        return await Task.FromResult(Ok());
    }

    [HttpDelete("{groupId}/delete-user")]
    public async Task<ActionResult> DeleteUsers(Guid groupId, [FromBody] DeleteUsersRequest request)
    {
        // Validate request
        if (request.UserIds == null || request.UserIds.Count == 0)
        {
            return BadRequest("No user provided for the group");
        }

        // Check if group exists
        var groupExists = true; // Replace with actual group check logic
        if (!groupExists)
        {
            return NotFound("Group not found");
        }

        // Check if all users exist
        var allUsersExist = true; // Replace with actual user check logic
        if (!allUsersExist)
        {
            return NotFound("User not found");
        }

        // Remove users from group
        // Implementation would go here

        return Ok();
    }

    [HttpGet("{groupId}/expenses")]
    public async Task<ActionResult<List<Expense>>> GetExpenses(Guid groupId)
    {
        // Check if group exists
        var groupExists = true; // Replace with actual group check logic
        if (!groupExists)
        {
            return NotFound("Group not found");
        }

        // Get expenses
        var expenses = new GetExpensesResponse
        {
            Expenses = new List<Expense>(),
            // Populate with actual expenses
        };

        return Ok(expenses);
    }

    [HttpGet("{groupId}/balances")]
    public async Task<ActionResult<List<UserBalanceItem>>> GetBalances(Guid groupId)
    {
        // Check if group exists
        var groupExists = true; // Replace with actual group check logic
        if (!groupExists)
        {
            return NotFound("Group not found");
        }

        // Get balances
        var balances = new GetBalancesResponse
        {
            UserBalances = new List<UserBalanceItem>(),
            // Populate with actual balances
        };

        return Ok(balances);
    }

    [HttpPost("{groupId}/settle")]
    public async Task<ActionResult> Settle(Guid groupId, [FromBody] SettleRequest request)
    {
        // Check if group exists
        var groupExists = true; // Replace with actual group check logic
        if (!groupExists)
        {
            return NotFound("Group not found");
        }

        // Validate request
        // if (Guid.TryParse(request.PayerUserId)) ||
        //     string.IsNullOrEmpty(request.ReceiverUserId) ||
        //     request.Amount <= 0)
        // {
        //     return BadRequest("Invalid settlement details");
        // }

        // Process settlement
        // Implementation would go here

        return Ok();
    }
}
