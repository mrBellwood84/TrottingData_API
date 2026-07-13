using Models.Entities;
using Models.Simple;

namespace API.Extensions;

public static class PolicyExtensions
{
    public static IServiceCollection AddEntityPolicies(this IServiceCollection services)
    {
        services.RegisterEntityPolicy<DriverLicenseEntity>(allowSimple: true, allowComplex: true);
        
        return services;
    }

    private static void RegisterEntityPolicy<T>(this IServiceCollection services, bool allowSimple, bool allowComplex)
    {
        services.AddSingleton(new EntityPolicy<T>
        {
            AllowAllSimple = allowSimple,
            AllowAllComplex = allowComplex,
        });
    }
}