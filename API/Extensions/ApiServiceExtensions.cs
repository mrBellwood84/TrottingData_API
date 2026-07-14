namespace API.Extensions;

/// <summary>
///     Extension methods for registering built-in ASP.NET Core API services.
/// </summary>
public static class ApiServiceExtensions
{
    /// <summary>
    ///     Registers core API services, including controllers and future framework configurations.
    /// </summary>
    /// <param name="services">The service collection to add the services to.</param>
    /// <returns>The updated <see cref="IServiceCollection" />.</returns>
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddOpenApi();

        return services;
    }
}