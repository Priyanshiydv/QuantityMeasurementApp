using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuantityMeasurement.Models.DTOs;
using QuantityMeasurement.Service.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

namespace QuantityMeasurement.WebAPI.Controllers
{
    /// <summary>
    /// User authentication controller.
    /// Handles Register, Login, Refresh, Google OAuth, Profile.
    /// UC18
    /// </summary>
    [ApiController]
    [Route("api/v1/users")]
    [Produces("application/json")]
    [Tags("① Authentication")]
    public class UserController : ControllerBase
    {
        private readonly IAuthService _auth;
        private readonly ILogger<UserController> _logger;

        public UserController(
            IAuthService auth,
            ILogger<UserController> logger)
        {
            _auth   = auth;

            _logger = logger;
        }

        // ── Register ──────────────────────────────────────────

        /// <summary>
        /// Register a new user.
        /// Password is BCrypt hashed (work factor 12) with
        /// auto-generated salt before saving to SQL Server.
        /// UC18
        /// </summary>
        [HttpPost("register")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(AuthResponseDTO), 201)]
        [ProducesResponseType(typeof(object), 409)]
        [SwaggerOperation(
            Summary     = "Register a new user",
            Description = "Hashes password with BCrypt (salt auto-embedded). Returns JWT + refresh token.")]
        public async Task<IActionResult> Register(
            [FromBody] RegisterRequestDTO req)
        {
            _logger.LogInformation(
                "[UserController] Register: {U}", req.Username);

            var result = await _auth.RegisterAsync(req);
            return StatusCode(201, result);
        }

        // ── Login ─────────────────────────────────────────────

        /// <summary>
        /// Login with username and password.
        /// Returns JWT access token (60 min) + refresh token (7 days).
        /// UC18
        /// </summary>
        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(AuthResponseDTO), 200)]
        [ProducesResponseType(typeof(object), 401)]
        [SwaggerOperation(
            Summary     = "Login with username and password",
            Description = "BCrypt verifies password. Returns JWT (HMAC-SHA256) + refresh token.")]
        public async Task<IActionResult> Login(
            [FromBody] LoginRequestDTO req)
        {
            _logger.LogInformation(
                "[UserController] Login: {U}", req.Username);

            var result = await _auth.LoginAsync(req);
            return Ok(result);
        }

        // ── Refresh Token ─────────────────────────────────────

        /// <summary>
        /// Refresh expired JWT using a valid refresh token.
        /// Rotates the refresh token on every call.
        /// UC18
        /// </summary>
        [HttpPost("refresh")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(AuthResponseDTO), 200)]
        [ProducesResponseType(typeof(object), 401)]
        [SwaggerOperation(
            Summary     = "Refresh JWT token",
            Description = "Validates refresh token, rotates it, and issues a new JWT.")]
        public async Task<IActionResult> Refresh(
            [FromBody] RefreshTokenRequestDTO req)
        {
            _logger.LogInformation(
                "[UserController] Token refresh requested.");

            var result = await _auth.RefreshTokenAsync(req);
            return Ok(result);
        }

        // ── Google OAuth ──────────────────────────────────────

        /// <summary>
        /// Login or auto-register via Google OAuth 2.0.
        /// Validates Google ID token, creates user if new.
        /// UC18
        /// </summary>
        [HttpPost("google-login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(AuthResponseDTO), 200)]
        [ProducesResponseType(typeof(object), 401)]
        [SwaggerOperation(
            Summary     = "Login with Google OAuth 2.0",
            Description = "Validates Google ID token. Auto-registers new Google users.")]
        public async Task<IActionResult> GoogleLogin(
            [FromBody] GoogleLoginRequestDTO req)
        {
            _logger.LogInformation(
                "[UserController] Google login attempt.");

            var result = await _auth.GoogleLoginAsync(req.IdToken);
            return Ok(result);
        }

        // ── Profile ───────────────────────────────────────────

        /// <summary>
        /// Get current user profile from JWT claims.
        /// Requires valid Bearer token in Authorization header.
        /// UC18
        /// </summary>
        [HttpGet("profile")]
        [Authorize]
        [ProducesResponseType(typeof(UserProfileDTO), 200)]
        [ProducesResponseType(401)]
        [SwaggerOperation(
            Summary     = "Get current user profile",
            Description = "Reads user info from JWT claims. Requires Bearer token.")]
        public IActionResult Profile()
        {
            _logger.LogInformation(
                "[UserController] Profile requested.");

            return Ok(new UserProfileDTO
            {
                Id       = int.Parse(
                    User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0"),
                Username = User.FindFirstValue(ClaimTypes.Name)  ?? "",
                Email    = User.FindFirstValue(ClaimTypes.Email) ?? "",
                Role     = User.FindFirstValue(ClaimTypes.Role)  ?? ""
            });
        }
    }
}