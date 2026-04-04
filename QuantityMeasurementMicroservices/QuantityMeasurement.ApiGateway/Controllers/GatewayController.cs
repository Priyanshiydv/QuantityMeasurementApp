using Microsoft.AspNetCore.Mvc;

namespace QuantityMeasurement.ApiGateway.Controllers
{
    [ApiController]
    [Route("api/v1")]
    [Produces("application/json")]
    public class GatewayController : ControllerBase
    {
        // ── Auth Routes ───────────────────────────────
        [HttpPost("auth/register")]
        [ProducesResponseType(200)]
        public IActionResult Register()
            => Ok(new { message = "Routed to Auth Service" });

        [HttpPost("auth/login")]
        [ProducesResponseType(200)]
        public IActionResult Login()
            => Ok(new { message = "Routed to Auth Service" });

        [HttpPost("auth/google-login")]
        [ProducesResponseType(200)]
        public IActionResult GoogleLogin()
            => Ok(new { message = "Routed to Auth Service" });

        [HttpPost("auth/refresh")]
        [ProducesResponseType(200)]
        public IActionResult Refresh()
            => Ok(new { message = "Routed to Auth Service" });

        [HttpGet("auth/profile")]
        [ProducesResponseType(200)]
        public IActionResult Profile()
            => Ok(new { message = "Routed to Auth Service" });

        // ── QMA Routes ────────────────────────────────
        [HttpPost("qma/compare")]
        [ProducesResponseType(200)]
        public IActionResult Compare()
            => Ok(new { message = "Routed to QMA Service" });

        [HttpPost("qma/convert")]
        [ProducesResponseType(200)]
        public IActionResult Convert()
            => Ok(new { message = "Routed to QMA Service" });

        [HttpPost("qma/add")]
        [ProducesResponseType(200)]
        public IActionResult Add()
            => Ok(new { message = "Routed to QMA Service" });

        [HttpPost("qma/subtract")]
        [ProducesResponseType(200)]
        public IActionResult Subtract()
            => Ok(new { message = "Routed to QMA Service" });

        [HttpPost("qma/divide")]
        [ProducesResponseType(200)]
        public IActionResult Divide()
            => Ok(new { message = "Routed to QMA Service" });

        [HttpGet("qma/history")]
        [ProducesResponseType(200)]
        public IActionResult GetHistory()
            => Ok(new { message = "Routed to QMA Service" });

        [HttpGet("qma/history/my")]
        [ProducesResponseType(200)]
        public IActionResult GetMyHistory()
            => Ok(new { message = "Routed to QMA Service" });

        [HttpGet("qma/count")]
        [ProducesResponseType(200)]
        public IActionResult GetCount()
            => Ok(new { message = "Routed to QMA Service" });
    }
}