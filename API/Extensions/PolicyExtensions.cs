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
        services.RegisterEntityPolicy<DriverLicenseEntity>(true);
        services.RegisterEntityPolicy<HorseSexEntity>(true);
        services.RegisterEntityPolicy<HorseTypeEntity>(true);
        services.RegisterEntityPolicy<RaceCartTypeEntity>(true);
        services.RegisterEntityPolicy<RaceCourseEntity>(true);
        services.RegisterEntityPolicy<RaceGamblingTypeEntity>(true);
        services.RegisterEntityPolicy<RaceStartTypeEntity>(true);

        return services;
    }

    /// <summary>
    ///     Registers a single <see cref="ModelPolicy{T}" /> as a singleton.
    /// </summary>
    /// <typeparam name="T">The entity type the policy applies to.</typeparam>
    /// <param name="services">The service collection.</param>
    /// <param name="allowGetAll">Specifies whether retrieving all entities of type <typeparamref name="T" /> is allowed.</param>
    private static void RegisterEntityPolicy<T>(this IServiceCollection services, bool allowGetAll)
    {
        services.AddSingleton(new ModelPolicy<T>
        {
            AllowGetAll = allowGetAll
        });
    }
}