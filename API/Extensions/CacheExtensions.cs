using Application.Cache.Services;

namespace API.Extensions;

/// <summary>
///     Extension methods for configuring in-memory caching services.
/// </summary>
public static class CacheExtensions
{
    /// <summary>
    ///     Registers the open generic cache service as a singleton in the Dependency Injection container.
    /// </summary>
    public static IServiceCollection AddCache(this IServiceCollection services)
    {
        services.AddSingleton(typeof(CacheService<>));
        return services;
    }
}