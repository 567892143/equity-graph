namespace EquityGraph.Api.Shared.Middleware;

using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Neo4j.Driver;

/// <summary>Global exception handling middleware translating errors to standardized HTTP responses.</summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    /// <summary>Initializes a new instance of ExceptionHandlingMiddleware.</summary>
    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>Processes the HTTP context and catches unhandled exceptions.</summary>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Neo4jException ex)
        {
            _logger.LogError(ex, "Database exception occurred: {Message}", ex.Message);
            await HandleExceptionAsync(context, StatusCodes.Status503ServiceUnavailable, new
            {
                error = "Database unavailable",
                detail = ex.Message
            });
        }
        catch (ArgumentOutOfRangeException ex)
        {
            _logger.LogWarning(ex, "Argument out of range: {Message}", ex.Message);
            await HandleExceptionAsync(context, StatusCodes.Status400BadRequest, new
            {
                error = "Invalid request",
                detail = ex.Message
            });
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Invalid argument: {Message}", ex.Message);
            await HandleExceptionAsync(context, StatusCodes.Status400BadRequest, new
            {
                error = "Invalid request",
                detail = ex.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception processing request {Path}", context.Request.Path);
            await HandleExceptionAsync(context, StatusCodes.Status500InternalServerError, new
            {
                error = "Internal server error"
            });
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, int statusCode, object responseBody)
    {
        if (context.Response.HasStarted)
        {
            return;
        }

        context.Response.Clear();
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json; charset=utf-8";

        await context.Response.WriteAsJsonAsync(responseBody);
    }
}
