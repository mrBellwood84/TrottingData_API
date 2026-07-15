using Models.Entity;
using Models.Shared;

namespace API.Extensions;

/// <summary>
///     Extension methods for configuring and registering entity model access policies.
/// </summary>
public static class PolicyExtensions
{
    /// <summary>
    ///     Registers all model access policies in the Dependency Injection container.
    /// </summary>
    /// <param name="services">The service collection to add the policies to.</param>
    /// <returns>The updated <see cref="IServiceCollection" />.</returns>
    public static IServiceCollection AddModelPolicies(this IServiceCollection services)
    {
        services.RegisterEntityPolicy<DriverLicenseEntity>(true, true);
        services.RegisterEntityPolicy<HorseSexEntity>(true, true);
        services.RegisterEntityPolicy<HorseTypeEntity>(true, true);
        services.RegisterEntityPolicy<RaceCartTypeEntity>(true, true);
        services.RegisterEntityPolicy<RaceCourseEntity>(true, true);
        services.RegisterEntityPolicy<RaceGamblingTypeEntity>(true, true);
        services.RegisterEntityPolicy<RaceStartTypeEntity>(true, true);

        return services;
    }

    /// <summary>
    ///     Registers a single <see cref="ModelPolicy{T}" /> as a singleton.
    /// </summary>
    /// <typeparam name="T">The entity type the policy applies to.</typeparam>
    /// <param name="services">The service collection.</param>
    /// <param name="allowIdList"></param>
    /// <param name="allowGetAll">Specifies whether retrieving all entities of type <typeparamref name="T" /> is allowed.</param>
    private static void RegisterEntityPolicy<T>(this IServiceCollection services, bool allowIdList, bool allowGetAll)
    {
        services.AddSingleton(new ModelPolicy<T>
        {
            AllowIdList = allowIdList,
            AllowGetAll = allowGetAll
        });
    }
}