using Models.Entities;
using Models.Simple;

namespace API.Extensions;

public static class PolicyExtensions
{
    public static IServiceCollection AddModelPolicies(this IServiceCollection services)
    {
        services.RegisterEntityPolicy<DriverLicenseEntity>(true, true);

        return services;
    }

    private static void RegisterEntityPolicy<T>(this IServiceCollection services, bool allowSimple, bool allowComplex)
    {
        services.AddSingleton(new ModelPolicy<T>
        {
            AllowAllSimple = allowSimple,
            AllowAllComplex = allowComplex
        });
    }
}