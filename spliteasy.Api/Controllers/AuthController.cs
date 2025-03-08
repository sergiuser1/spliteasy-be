using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SplitEasy.Models;
using spliteasy.Persistence;

namespace ExpenseSharingApp.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController(IAuthRepository authRepository) : ControllerBase
    {
        // [Authorize]
        [HttpGet("me")]
        public async Task<ActionResult<MeResponse>> Me()
        {
            // var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            // var username = User.Identity?.Name;

            // if (userId == null || username == null)
            // {
            //     return Unauthorized();
            // }

            return await Task.FromResult(
                Ok(new MeResponse { UserId = Guid.NewGuid(), Username = "" })
            );
        }

        [HttpGet("test")]
        public async Task<ActionResult<MeResponse>> Test(Guid userId)
        {
            var result = await authRepository.GetUserById(userId);

            return result is null ? NotFound() : Ok(result);
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

            var token = "real-token.jpg";
            var user = await authRepository.CreateUser(request.Username, request.Password);

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
            // Validate request
            if (
                string.IsNullOrWhiteSpace(request.Username)
                || string.IsNullOrWhiteSpace(request.Password)
            )
            {
                return BadRequest("Username and password are required");
            }

            // Check credentials
            var credentialsValid = true; // Replace with actual credential validation
            if (!credentialsValid)
            {
                return StatusCode(403, "Wrong combination of username/password");
            }

            // Generate token
            var token = "generated-jwt-token"; // Replace with actual token generation
            var userId = Guid.NewGuid();

            return Ok(new SignInResponse { UserId = userId, Token = token });
        }
    }
}
