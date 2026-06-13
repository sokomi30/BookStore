using BookStore.Application.DTOs;
using BookStore.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Serilog;

namespace BookStore.WebApi.Controllers
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

        // POST: api/auth/register
        [HttpPost("register")]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            try
            {
                var result = await _authService.RegisterAsync(dto);
                Log.Information("New user registered: {Username}", dto.Username);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                Log.Warning("Registration failed: {Message}", ex.Message);
                return Conflict(new { message = ex.Message });
            }
        }

        // POST: api/auth/login
        [EnableRateLimiting("LoginPolicy")]
        [HttpPost("login")]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var result = await _authService.LoginAsync(dto);
            if (result == null)
                return Unauthorized(new { message = "Invalid username or password" });
            Log.Information("User {Username} logged in", dto.Username);
            return Ok(result);
        }

        [HttpPost("refresh")]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenDto dto)
        {
            var result = await _authService.RefreshTokenAsync(dto.RefreshToken);
            if (result == null)
                return Unauthorized(new { message = "Invalid or expired refresh token" });

            return Ok(result);
        }
    }
}