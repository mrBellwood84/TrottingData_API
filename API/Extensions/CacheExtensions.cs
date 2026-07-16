using Application.Cache.Implementations;
using Application.Cache.Interfaces;

namespace API.Extensions;

public static class CacheExtensions
{
    /// <summary>
    ///     Registers open generic cache interfaces mapped to their implementations as singletons.
    /// </summary>
    public static IServiceCollection AddCache(this IServiceCollection services)
    {
        services.AddSingleton(typeof(IReadSingleCache<>), typeof(ReadReadSingleCache<>));
        services.AddSingleton(typeof(IReadAllCache<>), typeof(ReadAllCache<>));
        services.AddSingleton(typeof(IReadSourceCache<>), typeof(ReadSourceCache<>));

        return services;
    }
}