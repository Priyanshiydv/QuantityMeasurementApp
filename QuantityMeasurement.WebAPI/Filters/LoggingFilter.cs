using Microsoft.AspNetCore.Mvc.Filters;

namespace QuantityMeasurement.WebAPI.Filters
{
    /// <summary>
    /// Action filter that logs every API request and response.
    /// Runs before and after every controller action.
    /// Equivalent to @Around advice in Spring AOP.
    /// UC17
    /// </summary>
    public class LoggingFilter : IActionFilter
    {
        // ─── Fields ───────────────────────────────────────────

        /// <summary>
        /// Logger for filter operations.
        /// </summary>
        private readonly ILogger<LoggingFilter> _logger;

        // ─── Constructor ──────────────────────────────────────

        public LoggingFilter(ILogger<LoggingFilter> logger)
        {
            _logger = logger;
        }

        // ─── Before Action ────────────────────────────────────

        /// <summary>
        /// Runs BEFORE controller action executes.
        /// Logs incoming request details.
        /// UC17
        /// </summary>
        public void OnActionExecuting(
            ActionExecutingContext context)
        {
            string controllerName = context.RouteData
                .Values["controller"]?.ToString()
                ?? "Unknown";

            string actionName = context.RouteData
                .Values["action"]?.ToString()
                ?? "Unknown";

            string httpMethod = context.HttpContext
                .Request.Method;

            string path = context.HttpContext
                .Request.Path;

            _logger.LogInformation(
                "[LoggingFilter] → Request: " +
                "{HttpMethod} {Path} | " +
                "Controller: {Controller} | " +
                "Action: {Action}",
                httpMethod,
                path,
                controllerName,
                actionName);
        }

        // ─── After Action ─────────────────────────────────────

        /// <summary>
        /// Runs AFTER controller action executes.
        /// Logs response status code.
        /// UC17
        /// </summary>
        public void OnActionExecuted(
            ActionExecutedContext context)
        {
            string controllerName = context.RouteData
                .Values["controller"]?.ToString()
                ?? "Unknown";

            string actionName = context.RouteData
                .Values["action"]?.ToString()
                ?? "Unknown";

            int statusCode = context.HttpContext
                .Response.StatusCode;

            _logger.LogInformation(
                "[LoggingFilter] ← Response: " +
                "Status: {StatusCode} | " +
                "Controller: {Controller} | " +
                "Action: {Action}",
                statusCode,
                controllerName,
                actionName);
        }
    }
}