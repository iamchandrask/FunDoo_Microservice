using System.Net;
using System.Text.Json;

namespace Fundoo.UserService.API.Middleware;

// Custom Middleware:
// Handles all unhandled exceptions globally in the application.
// This middleware sits in the ASP.NET Core request pipeline
// and intercepts exceptions to return a structured JSON response.

// Benefits:
// - Centralized error handling
// - Consistent API responses
// - Prevents exposing sensitive stack traces to clients
// - Improves logging and debugging

public class ExceptionHandlingMiddleware
{
    // Delegate to call the next middleware in the pipeline.
    // - Represents the next component in the HTTP request pipeline.
    private readonly RequestDelegate _next;

    // Logger instance for logging exceptions.
    // - Helps in monitoring and debugging issues.
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    // Constructor Injection:
    // - RequestDelegate is automatically provided by ASP.NET Core.
    // - ILogger is injected via Dependency Injection.
    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    // Invoke Method:
    // - This method is called for every HTTP request.
    // - It wraps the request pipeline in a try-catch block.

    public async Task Invoke(HttpContext context)
    {
        try
        {
            // Pass the request to the next middleware/component.
            // - Could be routing, controller, etc.
            await _next(context);
        }
        catch (Exception ex)
        {
            // Log the exception details.
            // - Stores stack trace and message for debugging.
            _logger.LogError(ex, "Unhandled exception occurred");

            // Set response content type to JSON.
            context.Response.ContentType = "application/json";

            // Map exception types to HTTP status codes.
            // - Provides meaningful responses to client.
            context.Response.StatusCode = ex switch
            {
                // Authentication/Authorization failure
                UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized, // 401

                // Resource not found
                KeyNotFoundException => (int)HttpStatusCode.NotFound, // 404

                // Business validation error
                InvalidOperationException => (int)HttpStatusCode.BadRequest, // 400

                // Default case for unhandled exceptions
                _ => (int)HttpStatusCode.InternalServerError // 500
            };

            // Create a standardized error response object.
            var response = new
            {
                // HTTP status code (e.g., 400, 401, 500)
                StatusCode = context.Response.StatusCode,

                // Error message (can be customized for security)
                Message = ex.Message
            };

            // Serialize response object to JSON and send it to client.
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}