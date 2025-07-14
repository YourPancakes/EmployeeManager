using System.Net;
using System.Text.Json;

namespace EmployeeManager.Server.API.Middleware
{
    /// <summary>
    /// Middleware for handling exceptions globally across the application.
    /// Provides centralized exception handling and standardized error responses.
    /// </summary>
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private const string CONTENT_TYPE_JSON = "application/problem+json";

        /// <summary>
        /// Initializes a new instance of the ExceptionHandlingMiddleware with required dependencies.
        /// </summary>
        /// <param name="next">The next middleware in the request pipeline</param>
        /// <param name="logger">The logger instance for recording exceptions</param>
        /// <exception cref="ArgumentNullException">Thrown when next middleware or logger is null</exception>
        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Processes the HTTP request and handles any exceptions that occur during processing.
        /// </summary>
        /// <param name="context">The HTTP context for the current request</param>
        /// <returns>A task representing the asynchronous operation</returns>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "An unhandled exception occurred during request processing");
                await HandleExceptionAsync(context, exception);
            }
        }

        /// <summary>
        /// Handles the exception by creating an appropriate HTTP response with error details.
        /// </summary>
        /// <param name="context">The HTTP context for the current request</param>
        /// <param name="exception">The exception that occurred</param>
        /// <returns>A task representing the asynchronous operation</returns>
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = CONTENT_TYPE_JSON;

            var (statusCode, title, detail) = DetermineErrorResponse(exception);

            context.Response.StatusCode = (int)statusCode;

            var problemDetails = CreateProblemDetails(context, title, detail);

            var jsonResponse = JsonSerializer.Serialize(problemDetails, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            });

            await context.Response.WriteAsync(jsonResponse);
        }

        /// <summary>
        /// Determines the appropriate HTTP status code and error details based on the exception type.
        /// </summary>
        /// <param name="exception">The exception that occurred</param>
        /// <returns>A tuple containing status code, title, and detail</returns>
        private static (HttpStatusCode StatusCode, string Title, string Detail) DetermineErrorResponse(Exception exception)
        {
            return exception switch
            {
                ArgumentNullException _ => (HttpStatusCode.BadRequest, "Missing required argument", exception.Message),
                ArgumentException _ => (HttpStatusCode.BadRequest, "Invalid argument", exception.Message),
                InvalidOperationException _ => (HttpStatusCode.BadRequest, "Invalid operation", exception.Message),
                UnauthorizedAccessException _ => (HttpStatusCode.Unauthorized, "Unauthorized", "Access denied"),
                _ => (HttpStatusCode.InternalServerError, "Internal server error", "An unexpected error occurred")
            };
        }

        /// <summary>
        /// Creates a standardized problem details object for the error response.
        /// </summary>
        /// <param name="context">The HTTP context for the current request</param>
        /// <param name="title">The error title</param>
        /// <param name="detail">The error detail</param>
        /// <returns>An anonymous object containing problem details</returns>
        private static object CreateProblemDetails(HttpContext context, string title, string detail)
        {
            return new
            {
                type = $"{context.Response.StatusCode}",
                title,
                status = context.Response.StatusCode,
                detail,
                instance = context.Request.Path,
                timestamp = DateTime.UtcNow,
                traceId = context.TraceIdentifier
            };
        }
    }
}