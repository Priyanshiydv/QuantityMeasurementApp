using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using QuantityMeasurement.Models.Exceptions;

namespace QuantityMeasurement.WebAPI.Filters
{
    /// <summary>
    /// Action filter that validates model state.
    /// Runs before every controller action.
    /// Returns 400 Bad Request if model is invalid.
    /// UC17
    /// </summary>
    public class ValidationFilter : IActionFilter
    {
        // ─── Fields ───────────────────────────────────────────

        /// <summary>
        /// Logger for validation operations.
        /// </summary>
        private readonly ILogger<ValidationFilter> _logger;

        // ─── Constructor ──────────────────────────────────────

        public ValidationFilter(
            ILogger<ValidationFilter> logger)
        {
            _logger = logger;
        }

        // ─── Before Action ────────────────────────────────────

        /// <summary>
        /// Runs BEFORE controller action executes.
        /// Validates model state and returns 400 if invalid.
        /// UC17
        /// </summary>
        public void OnActionExecuting(
            ActionExecutingContext context)
        {
            // Check if model state is valid
            if (!context.ModelState.IsValid)
            {
                // Get all validation errors
                var errors = context.ModelState
                    .Where(e => e.Value?.Errors.Count > 0)
                    .SelectMany(e => e.Value!.Errors
                        .Select(err => new
                        {
                            Field   = e.Key,
                            Message = err.ErrorMessage
                        }))
                    .ToList();

                _logger.LogWarning(
                    "[ValidationFilter] " +
                    "Model validation failed: {Errors}",
                    errors);

                // Return 400 Bad Request
                context.Result = new BadRequestObjectResult(
                    new ErrorResponse
                    {
                        Timestamp = DateTime.Now,
                        Status    = 400,
                        Error     = "Validation Failed",
                        Message   = string.Join(", ",
                            errors.Select(e =>
                                $"{e.Field}: {e.Message}")),
                        Path      = context.HttpContext
                            .Request.Path,
                        ErrorCode = "VALIDATION_ERROR"
                    });
            }
        }

        // ─── After Action ─────────────────────────────────────

        /// <summary>
        /// Runs AFTER controller action executes.
        /// Nothing to do here for validation.
        /// UC17
        /// </summary>
        public void OnActionExecuted(
            ActionExecutedContext context)
        {
            // Nothing needed after action
        }
    }
}