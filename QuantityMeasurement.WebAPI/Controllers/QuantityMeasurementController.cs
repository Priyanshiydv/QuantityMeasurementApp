using Microsoft.AspNetCore.Mvc;
using QuantityMeasurement.Models.DTOs;
using QuantityMeasurement.Models.UnitDTOs;
using QuantityMeasurement.Repository.Interfaces;
using QuantityMeasurement.Service.Interfaces;
using QuantityMeasurement.Models.Exceptions;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace QuantityMeasurement.WebAPI.Controllers
{
    /// <summary>
    /// REST Controller for Quantity Measurement operations.
    /// Exposes all quantity measurement operations as REST API.
    /// Handles HTTP requests and returns JSON responses.
    /// UC17
    /// </summary>
    [ApiController]
    [Route("api/v1/quantities")]
    [Produces("application/json")]
    [Tags("② Quantity Measurement")]
    public class QuantityMeasurementController : ControllerBase
    {
        // ─── Fields ───────────────────────────────────────────

        /// <summary>
        /// Service for quantity measurement operations.
        /// Injected by ASP.NET Core DI Container.
        /// </summary>
        private readonly IQuantityMeasurementService _service;

        /// <summary>
        /// Repository for direct data access.
        /// Used for history and statistics endpoints.
        /// </summary>
        private readonly IQuantityMeasurementRepository
            _repository;

        /// <summary>
        /// Logger for controller operations.
        /// </summary>
        private readonly ILogger<QuantityMeasurementController>
            _logger;

        // ─── Constructor ──────────────────────────────────────

        /// <summary>
        /// Constructor with dependency injection.
        /// UC17
        /// </summary>
        public QuantityMeasurementController(
            IQuantityMeasurementService service,
            IQuantityMeasurementRepository repository,
            ILogger<QuantityMeasurementController> logger)
        {
            _service    = service;
            _repository = repository;
            _logger     = logger;

            _logger.LogInformation(
                "[Controller] Initialized ✓");
        }

        // ─── Get UserId from JWT ──────────────────────────

        /// <summary>
        /// Extracts UserId from JWT claims.
        /// Returns null if not authenticated.
        /// UC19
        /// </summary>
        private int? GetUserId()
        {
            var claim = User.FindFirstValue(
                ClaimTypes.NameIdentifier);
            return int.TryParse(claim, out int id) ? id : null;
        }

        // ─── Compare Endpoint ─────────────────────────────────

        /// <summary>
        /// Compares two quantities.
        /// POST /api/v1/quantities/compare
        /// UC17
        /// </summary>
        [HttpPost("compare")]
        [ProducesResponseType(typeof(QuantityResponseDTO),
            StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse),
            StatusCodes.Status400BadRequest)]
        [SwaggerOperation(
            Summary     = "Compare two quantities",
            Description = "Compares two quantities and returns true if equal")]
        public IActionResult CompareQuantities(
            [FromBody] QuantityInputDTO input)
        {
            _logger.LogInformation(
                "[Controller] CompareQuantities called.");

            QuantityDTO first = new QuantityDTO(
                input.FirstValue,
                input.FirstUnit,
                input.FirstMeasurementType);

            QuantityDTO second = new QuantityDTO(
                input.SecondValue,
                input.SecondUnit,
                input.SecondMeasurementType);

            QuantityDTO result = _service.Compare(first, second, GetUserId());

            QuantityResponseDTO response = new QuantityResponseDTO
            {
                FirstValue            = input.FirstValue,
                FirstUnit             = input.FirstUnit,
                FirstMeasurementType  = input.FirstMeasurementType,
                SecondValue           = input.SecondValue,
                SecondUnit            = input.SecondUnit,
                SecondMeasurementType = input.SecondMeasurementType,
                Operation             = "COMPARE",
                ResultString          = result.Unit,
                HasError              = false
            };

            return Ok(response);
        }

        // ─── Convert Endpoint ─────────────────────────────────

        /// <summary>
        /// Converts quantity to target unit.
        /// POST /api/v1/quantities/convert
        /// UC17
        /// </summary>
        [HttpPost("convert")]
        [ProducesResponseType(typeof(QuantityResponseDTO),
            StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse),
            StatusCodes.Status400BadRequest)]
        [SwaggerOperation(
            Summary     = "Convert quantity to target unit",
            Description = "Converts quantity from one unit to another")]
        public IActionResult ConvertQuantity(
            [FromBody] QuantityInputDTO input)
        {
            _logger.LogInformation(
                "[Controller] ConvertQuantity called.");

            string targetUnit = input.TargetUnit
                ?? input.SecondUnit;

            QuantityDTO quantity = new QuantityDTO(
                input.FirstValue,
                input.FirstUnit,
                input.FirstMeasurementType);

            QuantityDTO result = _service.Convert(quantity, targetUnit, GetUserId());

            QuantityResponseDTO response = new QuantityResponseDTO
            {
                FirstValue            = input.FirstValue,
                FirstUnit             = input.FirstUnit,
                FirstMeasurementType  = input.FirstMeasurementType,
                SecondValue           = input.SecondValue,
                SecondUnit            = input.SecondUnit,
                SecondMeasurementType = input.SecondMeasurementType,
                Operation             = "CONVERT",
                ResultValue           = result.Value,
                ResultUnit            = result.Unit,
                ResultMeasurementType = result.MeasurementType,
                HasError              = false
            };

            return Ok(response);
        }

        // ─── Add Endpoint ─────────────────────────────────────

        /// <summary>
        /// Adds two quantities.
        /// POST /api/v1/quantities/add
        /// UC17
        /// </summary>
        [HttpPost("add")]
        [ProducesResponseType(typeof(QuantityResponseDTO),
            StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse),
            StatusCodes.Status400BadRequest)]
        [SwaggerOperation(
            Summary     = "Add two quantities",
            Description = "Adds two quantities and returns result in first or target unit")]
        public IActionResult AddQuantities(
            [FromBody] QuantityInputDTO input)
        {
            _logger.LogInformation(
                "[Controller] AddQuantities called.");

            QuantityDTO first = new QuantityDTO(
                input.FirstValue,
                input.FirstUnit,
                input.FirstMeasurementType);

            QuantityDTO second = new QuantityDTO(
                input.SecondValue,
                input.SecondUnit,
                input.SecondMeasurementType);

            QuantityDTO result = input.TargetUnit != null
                ? _service.Add(first, second, input.TargetUnit, GetUserId())
                : _service.Add(first, second);

            QuantityResponseDTO response = new QuantityResponseDTO
            {
                FirstValue            = input.FirstValue,
                FirstUnit             = input.FirstUnit,
                FirstMeasurementType  = input.FirstMeasurementType,
                SecondValue           = input.SecondValue,
                SecondUnit            = input.SecondUnit,
                SecondMeasurementType = input.SecondMeasurementType,
                Operation             = "ADD",
                ResultValue           = result.Value,
                ResultUnit            = result.Unit,
                ResultMeasurementType = result.MeasurementType,
                HasError              = false
            };

            return Ok(response);
        }

        // ─── Subtract Endpoint ────────────────────────────────

        /// <summary>
        /// Subtracts two quantities.
        /// POST /api/v1/quantities/subtract
        /// UC17
        /// </summary>
        [HttpPost("subtract")]
        [ProducesResponseType(typeof(QuantityResponseDTO),
            StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse),
            StatusCodes.Status400BadRequest)]
        [SwaggerOperation(
            Summary     = "Subtract two quantities",
            Description = "Subtracts second quantity from first quantity")]
        public IActionResult SubtractQuantities(
            [FromBody] QuantityInputDTO input)
        {
            _logger.LogInformation(
                "[Controller] SubtractQuantities called.");

            QuantityDTO first = new QuantityDTO(
                input.FirstValue,
                input.FirstUnit,
                input.FirstMeasurementType);

            QuantityDTO second = new QuantityDTO(
                input.SecondValue,
                input.SecondUnit,
                input.SecondMeasurementType);

            QuantityDTO result = _service.Subtract(first, second, GetUserId());

            QuantityResponseDTO response = new QuantityResponseDTO
            {
                FirstValue            = input.FirstValue,
                FirstUnit             = input.FirstUnit,
                FirstMeasurementType  = input.FirstMeasurementType,
                SecondValue           = input.SecondValue,
                SecondUnit            = input.SecondUnit,
                SecondMeasurementType = input.SecondMeasurementType,
                Operation             = "SUBTRACT",
                ResultValue           = result.Value,
                ResultUnit            = result.Unit,
                ResultMeasurementType = result.MeasurementType,
                HasError              = false
            };

            return Ok(response);
        }

        // ─── Divide Endpoint ──────────────────────────────────

        /// <summary>
        /// Divides two quantities.
        /// POST /api/v1/quantities/divide
        /// UC17
        /// </summary>
        [HttpPost("divide")]
        [ProducesResponseType(typeof(QuantityResponseDTO),
            StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse),
            StatusCodes.Status400BadRequest)]
        [SwaggerOperation(
            Summary     = "Divide two quantities",
            Description = "Divides first quantity by second and returns scalar result")]
        public IActionResult DivideQuantities(
            [FromBody] QuantityInputDTO input)
        {
            _logger.LogInformation(
                "[Controller] DivideQuantities called.");

            QuantityDTO first = new QuantityDTO(
                input.FirstValue,
                input.FirstUnit,
                input.FirstMeasurementType);

            QuantityDTO second = new QuantityDTO(
                input.SecondValue,
                input.SecondUnit,
                input.SecondMeasurementType);

            QuantityDTO result = _service.Divide(first, second, GetUserId());

            QuantityResponseDTO response = new QuantityResponseDTO
            {
                FirstValue            = input.FirstValue,
                FirstUnit             = input.FirstUnit,
                FirstMeasurementType  = input.FirstMeasurementType,
                SecondValue           = input.SecondValue,
                SecondUnit            = input.SecondUnit,
                SecondMeasurementType = input.SecondMeasurementType,
                Operation             = "DIVIDE",
                ResultValue           = result.Value,
                ResultUnit            = "SCALAR",
                HasError              = false
            };

            return Ok(response);
        }

        // ─── History Endpoints ────────────────────────────────

        /// <summary>
        /// Returns all measurement history.
        /// GET /api/v1/quantities/history
        /// UC17
        /// </summary>
        [HttpGet("history")]
        [ProducesResponseType(
            typeof(List<QuantityResponseDTO>),
            StatusCodes.Status200OK)]      
        [SwaggerOperation(
            Summary     = "Get all measurement history",
            Description = "Returns all quantity measurement operations history")]
        public IActionResult GetAllHistory()
        {
            _logger.LogInformation(
                "[Controller] GetAllHistory called.");

            var entities = _repository.GetAllMeasurements();
            var response =
                QuantityResponseDTO.FromEntityList(entities);

            return Ok(response);
        }

        /// <summary>
        /// Returns history by operation type.
        /// GET /api/v1/quantities/history/operation/{type}
        /// UC17
        /// </summary>
        [HttpGet("history/operation/{operationType}")]
        [ProducesResponseType(
            typeof(List<QuantityResponseDTO>),
            StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary     = "Get history by operation type",
            Description = "Filter by COMPARE, CONVERT, ADD, SUBTRACT, DIVIDE")]
        public IActionResult GetHistoryByOperation(
            string operationType)
        {
            _logger.LogInformation(
                "[Controller] GetHistoryByOperation: {Type}",
                operationType);

            var entities =
                _repository.GetMeasurementsByOperationType(
                    operationType);

            var response =
                QuantityResponseDTO.FromEntityList(entities);

            return Ok(response);
        }

        /// <summary>
        /// Returns history by measurement type.
        /// GET /api/v1/quantities/history/measurement/{type}
        /// UC17
        /// </summary>
        [HttpGet("history/measurement/{measurementType}")]
        [ProducesResponseType(
            typeof(List<QuantityResponseDTO>),
            StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary     = "Get history by measurement type",
            Description = "Filter by Length, Weight, Volume, Temperature")]
        public IActionResult GetHistoryByMeasurement(
            string measurementType)
        {
            _logger.LogInformation(
                "[Controller] GetHistoryByMeasurement: {Type}",
                measurementType);

            var entities =
                _repository.GetMeasurementsByMeasurementType(
                    measurementType);

            var response =
                QuantityResponseDTO.FromEntityList(entities);

            return Ok(response);
        }

        /// <summary>
        /// Returns count by operation type.
        /// GET /api/v1/quantities/count/{operationType}
        /// UC17
        /// </summary>
        [HttpGet("count/{operationType}")]
        [ProducesResponseType(typeof(object),
            StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary     = "Get count by operation type",
            Description = "Returns total count of operations by type")]
        public IActionResult GetOperationCount(
            string operationType)
        {
            _logger.LogInformation(
                "[Controller] GetOperationCount: {Type}",
                operationType);

            int count = _repository
                .GetCountByOperationType(operationType);

            return Ok(new
            {
                OperationType = operationType,
                Count         = count
            });
        }

        /// <summary>
        /// Returns all error history.
        /// GET /api/v1/quantities/errors
        /// UC17
        /// </summary>
        [HttpGet("errors")]
        [ProducesResponseType(
            typeof(List<QuantityResponseDTO>),
            StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary     = "Get error history",
            Description = "Returns all failed quantity measurement operations")]
        public IActionResult GetErrorHistory()
        {
            _logger.LogInformation(
                "[Controller] GetErrorHistory called.");

            var entities = _repository.GetErrorMeasurements();
            var response =
                QuantityResponseDTO.FromEntityList(entities);

            return Ok(response);
        }

        /// <summary>
        /// Returns total count of all measurements.
        /// GET /api/v1/quantities/count
        /// UC17
        /// </summary>
        [HttpGet("count")]
        [ProducesResponseType(typeof(object),
            StatusCodes.Status200OK)]  
        [SwaggerOperation(
            Summary     = "Get total measurements count",
            Description = "Returns total count of all measurements in database")]
        public IActionResult GetTotalCount()
        {
            _logger.LogInformation(
                "[Controller] GetTotalCount called.");

            int count = _repository.GetTotalCount();

            return Ok(new { TotalCount = count });
        }

        /// <summary>
        /// Returns history for logged in user only.
        /// GET /api/v1/quantities/history/my
        /// UC19
        /// </summary>
        [HttpGet("history/my")]
        [Authorize]
        [ProducesResponseType(
            typeof(List<QuantityResponseDTO>),
            StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary     = "Get my history",
            Description = "Returns history for currently logged in user only")]
        public IActionResult GetMyHistory()
        {
            _logger.LogInformation(
                "[Controller] GetMyHistory called.");

            int? userId = GetUserId();
            if (userId == null)
                return Unauthorized();

            var entities = _repository
                .GetMeasurementsByUserId(userId.Value);
            var response =
                QuantityResponseDTO.FromEntityList(entities);

            return Ok(response);
        }
    }
}