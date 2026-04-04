using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using QuantityMeasurement.AuthService.Interfaces;
using QuantityMeasurement.AuthService.Models;

namespace QuantityMeasurement.AuthService.Controllers
{
    [ApiController]
    [Route("api/v1/auth")]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            IAuthService authService,
            ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger      = logger;
        }

        // ── Register ──────────────────────────────────
        [HttpPost("register")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(AuthResponse), 201)]
        public async Task<IActionResult> Register(
            [FromBody] RegisterRequest req)
        {
            _logger.LogInformation(
                "[AuthController] Register: {U}",
                req.Username);

            var result = await _authService
                .RegisterAsync(req);
            return StatusCode(201, result);
        }

        // ── Login ─────────────────────────────────────
        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(AuthResponse), 200)]
        public async Task<IActionResult> Login(
            [FromBody] LoginRequest req)
        {
            _logger.LogInformation(
                "[AuthController] Login: {U}",
                req.Username);

            var result = await _authService
                .LoginAsync(req);
            return Ok(result);
        }

        // ── Refresh Token ─────────────────────────────
        [HttpPost("refresh")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(AuthResponse), 200)]
        public async Task<IActionResult> Refresh(
            [FromBody] RefreshTokenRequest req)
        {
            _logger.LogInformation(
                "[AuthController] Token refresh requested.");

            var result = await _authService
                .RefreshTokenAsync(req);
            return Ok(result);
        }

        // ── Google Login ──────────────────────────────
        [HttpPost("google-login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(AuthResponse), 200)]
        public async Task<IActionResult> GoogleLogin(
            [FromBody] GoogleLoginRequest req)
        {
            _logger.LogInformation(
                "[AuthController] Google login attempt.");

            var result = await _authService
                .GoogleLoginAsync(req.AccessToken);
            return Ok(result);
        }

        // ── Profile ───────────────────────────────────
        [HttpGet("profile")]
        [Authorize]
        [ProducesResponseType(
            typeof(UserProfileResponse), 200)]
        public IActionResult Profile()
        {
            _logger.LogInformation(
                "[AuthController] Profile requested.");

            return Ok(new UserProfileResponse
            {
                Id = int.Parse(
                    User.FindFirstValue(
                        ClaimTypes.NameIdentifier) ?? "0"),
                Username = User.FindFirstValue(
                    ClaimTypes.Name)  ?? "",
                Email    = User.FindFirstValue(
                    ClaimTypes.Email) ?? "",
                Role     = User.FindFirstValue(
                    ClaimTypes.Role)  ?? ""
            });
        }
    }
}