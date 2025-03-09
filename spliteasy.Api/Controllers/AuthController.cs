using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SplitEasy.Models;
using spliteasy.Persistence;

namespace ExpenseSharingApp.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController(IConfiguration configuration, IAuthRepository authRepository)
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

            var user = await authRepository.GetUser(request.Username, request.Password);

            if (user == null)
            {
                return Unauthorized("Invalid username or password");
            }

            // Generate JWT token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Name, user.Username),
                    }
                ),
                Expires = DateTime.UtcNow.AddHours(4),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                ),
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new SignInResponse { UserId = user.Id, Token = tokenString });
        }
    }
}
