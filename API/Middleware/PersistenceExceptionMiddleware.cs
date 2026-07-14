using System.Text.Json;
using Application.Repository.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace API.Middleware;

/// <summary>
///     Global middleware to intercept, log, and gracefully handle database and persistence exceptions.
/// </summary>
/// <remarks>
///     Converts caught exceptions into standardized RFC 7807 Problem Details responses.
/// </remarks>
public class PersistenceExceptionMiddleware(RequestDelegate next, ILogger<PersistenceExceptionMiddleware> logger)
{
    /// <summary>
    ///     Invokes the middleware to process the current HTTP context.
    /// </summary>
    /// <param name="context">The current HTTP context.</param>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (RepositoryPolicyViolationException ex)
        {
            logger.LogWarning($"Persistence query blocked by policy: {ex.Message}");

            await WriteProblemDetailsResponseAsync(
                context,
                StatusCodes.Status403Forbidden,
                "Access Forbidden",
                ex.Message
            );
        }
        catch (Exception ex)
        {
            // Log the full exception details internally for debugging
            logger.LogError(ex, "An unhandled exception occurred in the persistence layer.");

            // Return a generic, safe message to the client to avoid leaking sensitive database schemas or connection details
            await WriteProblemDetailsResponseAsync(
                context,
                StatusCodes.Status500InternalServerError,
                "Internal Server Error",
                "An unexpected error occurred while processing your request. Please try again later."
            );
        }
    }

    /// <summary>
    ///     Writes a standardized RFC 7807 Problem Details JSON response to the HTTP output stream.
    /// </summary>
    private static async Task WriteProblemDetailsResponseAsync(HttpContext context, int statusCode, string title,
        string detail)
    {
        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = statusCode;

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = detail,
            Instance = context.Request.Path
        };

        var json = JsonSerializer.Serialize(problemDetails);
        await context.Response.WriteAsync(json);
    }
}