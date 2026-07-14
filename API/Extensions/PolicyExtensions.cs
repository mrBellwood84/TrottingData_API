using Models.Entities;
using Models.Simple;

namespace API.Extensions;

public static class PolicyExtensions
{
    public static IServiceCollection AddModelPolicies(this IServiceCollection services)
    {
        services.RegisterEntityPolicy<DriverLicenseEntity>(true);
        services.RegisterEntityPolicy<HorseSexEntity>(true);
        services.RegisterEntityPolicy<HorseTypeEntity>(true);
        services.RegisterEntityPolicy<RaceCartTypeEntity>(true);
        services.RegisterEntityPolicy<RaceCourseEntity>(true);
        services.RegisterEntityPolicy<RaceGamblingTypeEntity>(true);
        services.RegisterEntityPolicy<RaceStartTypeEntity>(true);

        return services;
    }

    private static void RegisterEntityPolicy<T>(this IServiceCollection services, bool allowGetAll)
    {
        services.AddSingleton(new ModelPolicy<T>
        {
            AllowGetAll = allowGetAll
        });
    }
}