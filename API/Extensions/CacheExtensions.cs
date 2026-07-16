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
        services.AddSingleton(typeof(ISingleItemCache<>), typeof(SingleItemCache<>));
        services.AddSingleton(typeof(IListItemCache<>), typeof(ListItemCache<>));
        services.AddSingleton(typeof(ISourceItemCache<>), typeof(SourceItemCache<>));

        return services;
    }
}