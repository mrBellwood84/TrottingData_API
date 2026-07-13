using Application.Cache.Services;

namespace API.Extensions;

public static class CacheExtensions
{
    public static IServiceCollection AddCache(this IServiceCollection services)
    {
        services.AddSingleton(typeof(CacheService<>));
        return services;
    }
}