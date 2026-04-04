using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using QuantityMeasurement.QMAService.Data;
using QuantityMeasurement.QMAService.Interfaces;
using QuantityMeasurement.QMAService.Models;

namespace QuantityMeasurement.QMAService.Controllers
{
    [ApiController]
    [Route("api/v1/qma")]
    [Produces("application/json")]
    public class QMAController : ControllerBase
    {
        private readonly IQMAService _service;
        private readonly QMADbContext _db;
        private readonly ILogger<QMAController> _logger;

        public QMAController(
            IQMAService service,
            QMADbContext db,
            ILogger<QMAController> logger)
        {
            _service = service;
            _db      = db;
            _logger  = logger;
        }

        // ── Get UserId from JWT ───────────────────────
        private int? GetUserId()
        {
            var claim = User.FindFirstValue(
                ClaimTypes.NameIdentifier);
            return int.TryParse(claim, out int id)
                ? id : null;
        }

        // ── Compare ───────────────────────────────────
        [HttpPost("compare")]
        [ProducesResponseType(
            typeof(QuantityResponse), 200)]
        public IActionResult Compare(
            [FromBody] QuantityInput input)
        {
            _logger.LogInformation(
                "[QMAController] Compare called.");
            var result = _service.Compare(
                input, GetUserId());
            return Ok(result);
        }

        // ── Convert ───────────────────────────────────
        [HttpPost("convert")]
        [ProducesResponseType(
            typeof(QuantityResponse), 200)]
        public IActionResult Convert(
            [FromBody] QuantityInput input)
        {
            _logger.LogInformation(
                "[QMAController] Convert called.");
            var result = _service.Convert(
                input, GetUserId());
            return Ok(result);
        }

        // ── Add ───────────────────────────────────────
        [HttpPost("add")]
        [ProducesResponseType(
            typeof(QuantityResponse), 200)]
        public IActionResult Add(
            [FromBody] QuantityInput input)
        {
            _logger.LogInformation(
                "[QMAController] Add called.");
            var result = _service.Add(
                input, GetUserId());
            return Ok(result);
        }

        // ── Subtract ──────────────────────────────────
        [HttpPost("subtract")]
        [ProducesResponseType(
            typeof(QuantityResponse), 200)]
        public IActionResult Subtract(
            [FromBody] QuantityInput input)
        {
            _logger.LogInformation(
                "[QMAController] Subtract called.");
            var result = _service.Subtract(
                input, GetUserId());
            return Ok(result);
        }

        // ── Divide ────────────────────────────────────
        [HttpPost("divide")]
        [ProducesResponseType(
            typeof(QuantityResponse), 200)]
        public IActionResult Divide(
            [FromBody] QuantityInput input)
        {
            _logger.LogInformation(
                "[QMAController] Divide called.");
            var result = _service.Divide(
                input, GetUserId());
            return Ok(result);
        }

        // ── Get All History ───────────────────────────
        [HttpGet("history")]
        [Authorize]
        [ProducesResponseType(
            typeof(List<HistoryResponse>), 200)]
        public IActionResult GetAllHistory()
        {
            _logger.LogInformation(
                "[QMAController] GetAllHistory called.");

            var entities = _db.Measurements
                .OrderByDescending(e => e.Timestamp)
                .ToList();

            return Ok(MapToHistory(entities));
        }

        // ── Get My History ────────────────────────────
        [HttpGet("history/my")]
        [Authorize]
        [ProducesResponseType(
            typeof(List<HistoryResponse>), 200)]
        public IActionResult GetMyHistory()
        {
            _logger.LogInformation(
                "[QMAController] GetMyHistory called.");

            int? userId = GetUserId();
            if (userId == null) return Unauthorized();

            var entities = _db.Measurements
                .Where(e => e.UserId == userId)
                .OrderByDescending(e => e.Timestamp)
                .ToList();

            return Ok(MapToHistory(entities));
        }

        // ── Get History By Operation ──────────────────
        [HttpGet("history/my/operation/{type}")]
        [Authorize]
        [ProducesResponseType(
            typeof(List<HistoryResponse>), 200)]
        public IActionResult GetMyHistoryByOperation(
            string type)
        {
            int? userId = GetUserId();
            if (userId == null) return Unauthorized();

            var entities = _db.Measurements
                .Where(e => e.UserId == userId &&
                    e.OperationType.ToUpper() ==
                    type.ToUpper())
                .OrderByDescending(e => e.Timestamp)
                .ToList();

            return Ok(MapToHistory(entities));
        }

        // ── Get History By Measurement ────────────────
        [HttpGet("history/my/measurement/{type}")]
        [Authorize]
        [ProducesResponseType(
            typeof(List<HistoryResponse>), 200)]
        public IActionResult GetMyHistoryByMeasurement(
            string type)
        {
            int? userId = GetUserId();
            if (userId == null) return Unauthorized();

            var entities = _db.Measurements
                .Where(e => e.UserId == userId &&
                    e.MeasurementType != null &&
                    e.MeasurementType.ToUpper() ==
                    type.ToUpper())
                .OrderByDescending(e => e.Timestamp)
                .ToList();

            return Ok(MapToHistory(entities));
        }

        // ── Get Count ─────────────────────────────────
        [HttpGet("count")]
        [ProducesResponseType(typeof(object), 200)]
        public IActionResult GetCount()
        {
            int count = _db.Measurements.Count();
            return Ok(new { TotalCount = count });
        }

        // ── Map Helper ────────────────────────────────
        private List<HistoryResponse> MapToHistory(
            List<MeasurementEntity> entities)
        {
            return entities.Select(e =>
                new HistoryResponse
                {
                    Operation   = e.OperationType,
                    FirstUnit   = e.FirstOperand,
                    SecondUnit  = e.SecondOperand,
                    ResultString = e.Result,
                    FirstMeasurementType = e.MeasurementType,
                    HasError    = e.HasError,
                    ErrorMessage = e.ErrorMessage,
                    Timestamp   = e.Timestamp
                }).ToList();
        }
    }
}