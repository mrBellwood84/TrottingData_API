using API.Middleware;

namespace API.Extensions;

/// <summary>
///     Extension methods for configuring the HTTP request pipeline.
/// </summary>
public static class PipelineExtensions
{
    /// <summary>
    ///     Configures the middleware pipeline and endpoints for the API.
    /// </summary>
    /// <param name="app">The web application to configure.</param>
    /// <returns>The configured <see cref="WebApplication" />.</returns>
    public static WebApplication UseApiPipeline(this WebApplication app)
    {
        app.UseMiddleware<PersistenceExceptionMiddleware>();

        app.MapControllers();

        return app;
    }
}