using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SplitEasy.Models;

namespace ExpenseSharingApp.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        [Authorize]
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

        [HttpPost("sign-up")]
        public async Task<ActionResult<SignUpResponse>> SignUp([FromBody] SignUpRequest request)
        {
            // Validate request
            if (
                string.IsNullOrWhiteSpace(request.Username)
                || string.IsNullOrWhiteSpace(request.Password)
            )
            {
                return BadRequest("Username and password are required");
            }

            // Check if user already exists
            var userExists = false; // Replace with actual user check logic
            if (userExists)
            {
                return StatusCode(401, "User already exists");
            }

            // Create user and generate token
            var token = "generated-jwt-token"; // Replace with actual token generation

            return Ok(new SignUpResponse { Token = token });
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
