using backend.Application.Dtos.Auth;
using backend.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace backend.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet("signin")]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return Challenge("Steam");
        }

        [AllowAnonymous]
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.RefreshToken))
                return BadRequest(new { message = "Refresh token is required" });

            var tokens = await _authService.RefreshAsync(request.RefreshToken, cancellationToken);
            if (tokens == null)
                return Unauthorized(new { message = "Invalid or expired refresh token" });

            return Ok(tokens);
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetMe(CancellationToken cancellationToken)
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier)
                              ?? User.FindFirstValue("sub");

            if (!int.TryParse(userIdValue, out var userId))
                return Unauthorized(new { message = "Unauthorized" });

            var user = await _authService.GetMeAsync(userId, cancellationToken);
            if (user == null)
                return NotFound(new { message = "User not found" });

            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] RefreshTokenRequestDto? request, CancellationToken cancellationToken)
        {
            int? userId = null;
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier)
                              ?? User.FindFirstValue("sub");

            if (int.TryParse(userIdValue, out var parsedId))
                userId = parsedId;

            await _authService.LogoutAsync(userId, request?.RefreshToken, cancellationToken);
            return Ok(new { message = "Logged out successfully" });
        }
    }
}
