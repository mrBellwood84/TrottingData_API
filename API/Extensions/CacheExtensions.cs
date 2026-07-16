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
        // generic cache services added!
        services.AddSingleton(typeof(IReadSingleCache<>), typeof(ReadReadSingleCache<>));
        services.AddSingleton(typeof(IReadAllCache<>), typeof(ReadAllCache<>));
        services.AddSingleton(typeof(IReadSourceCache<>), typeof(ReadSourceCache<>));
        
        // advanced cache services :)
        services.AddSingleton(typeof(IRaceCache<>), typeof(RaceCache<>));
        services.AddSingleton(typeof(IRaceParticipantCache),  typeof(RaceParticipantCache));
        services.AddSingleton(typeof(IRaceResultCache), typeof(RaceResultCache));

        return services;
    }
}