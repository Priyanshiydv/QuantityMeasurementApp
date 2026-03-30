using System.Net;
using System.Text.Json;
using QuantityMeasurement.Models.Exceptions;

namespace QuantityMeasurement.WebAPI.Exceptions
{
    /// <summary>
    /// Global exception handling middleware.
    /// Catches all unhandled exceptions from controllers.
    /// Returns consistent JSON error responses.
    /// Equivalent to @ControllerAdvice in Spring Boot.
    /// UC17
    /// </summary>
    public class GlobalExceptionMiddleware
    {
        // ─── Fields ───────────────────────────────────────────

        /// <summary>
        /// Next middleware in pipeline.
        /// </summary>
        private readonly RequestDelegate _next;

        /// <summary>
        /// Logger for exception logging.
        /// </summary>
        private readonly ILogger<GlobalExceptionMiddleware>
            _logger;

        // ─── Constructor ──────────────────────────────────────

        public GlobalExceptionMiddleware(
            RequestDelegate next,
            ILogger<GlobalExceptionMiddleware> logger)
        {
            _next   = next;
            _logger = logger;
        }

        // ─── Middleware Invoke ────────────────────────────────

        /// <summary>
        /// Invokes middleware and catches exceptions.
        /// Called for every HTTP request.
        /// UC17
        /// </summary>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (DatabaseException ex)
            {
                // Catch DatabaseException FIRST
                // because it extends QuantityMeasurementException
                _logger.LogError(
                    "[GlobalExceptionMiddleware] " +
                    "DatabaseException: {Message}",
                    ex.Message);

                await HandleExceptionAsync(
                    context,
                    ex,
                    HttpStatusCode.InternalServerError);
            }
            catch (QuantityMeasurementException ex)
            {
                // Catch parent AFTER child exception
                _logger.LogWarning(
                    "[GlobalExceptionMiddleware] " +
                    "QuantityMeasurementException: {Message}",
                    ex.Message);

                await HandleExceptionAsync(
                    context,
                    ex,
                    HttpStatusCode.BadRequest);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(
                    "[GlobalExceptionMiddleware] " +
                    "ArgumentException: {Message}",
                    ex.Message);

                await HandleExceptionAsync(
                    context,
                    ex,
                    HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    "[GlobalExceptionMiddleware] " +
                    "Unexpected Exception: {Message}",
                    ex.Message);

                await HandleExceptionAsync(
                    context,
                    ex,
                    HttpStatusCode.InternalServerError);
            }
        }

        // ─── Private Helper ───────────────────────────────────

        /// <summary>
        /// Writes JSON error response to HTTP context.
        /// Returns consistent error format for all exceptions.
        /// UC17
        /// </summary>
        private static async Task HandleExceptionAsync(
            HttpContext context,
            Exception exception,
            HttpStatusCode statusCode)
        {
            context.Response.ContentType =
                "application/json";

            context.Response.StatusCode =
                (int)statusCode;

            var errorResponse = new ErrorResponse
            {
                Timestamp = DateTime.Now,
                Status    = (int)statusCode,
                Error     = statusCode.ToString(),
                Message   = exception.Message,
                Path      = context.Request.Path,
                ErrorCode = exception is
                    QuantityMeasurementException qme
                    ? qme.ErrorCode
                    : "INTERNAL_ERROR"
            };

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy =
                    JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };

            string jsonResponse = JsonSerializer.Serialize(
                errorResponse, jsonOptions);

            await context.Response.WriteAsync(jsonResponse);
        }
    }
}