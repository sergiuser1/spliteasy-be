using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using spliteasy.Auth;
using SplitEasy.Models;
using spliteasy.Persistence;

namespace ExpenseSharingApp.Controllers;

[ApiController]
[Route("auth")]
public class AuthController(IAuthService authService, IAuthRepository authRepository)
    : ControllerBase
{
    [Authorize]
    [HttpGet("me")]
    public async Task<ActionResult<MeResponse>> Me()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var username = User.Identity?.Name;

        if (userId == null || username == null)
        {
            return Unauthorized();
        }

        return await Task.FromResult(
            Ok(new MeResponse { UserId = Guid.Parse(userId), Username = username })
        );
    }

    [HttpPost("sign-up")]
    public async Task<ActionResult<SignUpResponse>> SignUp([FromBody] SignUpRequest request)
    {
        if (
            string.IsNullOrWhiteSpace(request.Username)
            || string.IsNullOrWhiteSpace(request.Password)
        )
        {
            return BadRequest("Username and password are required");
        }

        var user = await authRepository.CreateUser(request.Username, request.Password);
        var token = authService.GenerateToken(user);

        return Ok(
            new SignUpResponse
            {
                Username = user.Username,
                UserId = user.Id,
                Token = token,
            }
        );
    }

    [HttpPost("sign-in")]
    public async Task<ActionResult<SignInResponse>> SignIn([FromBody] SignInRequest request)
    {
        if (
            string.IsNullOrWhiteSpace(request.Username)
            || string.IsNullOrWhiteSpace(request.Password)
        )
        {
            return BadRequest("Username and password are required");
        }

        var user = await authRepository.GetUser(request.Username, request.Password);

        if (user == null)
        {
            return Unauthorized("Invalid username or password");
        }

        var tokenString = authService.GenerateToken(user);

        return Ok(new SignInResponse { UserId = user.Id, Token = tokenString });
    }
}
