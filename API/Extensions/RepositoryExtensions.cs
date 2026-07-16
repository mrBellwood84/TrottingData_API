using Application.Repository.Implementations;
using Application.Repository.Interfaces;
using Models.Complex;
using Models.Entity;

namespace API.Extensions;

/// <summary>
///     Extension methods for configuring and registering repository services.
/// </summary>
public static class RepositoryExtensions
{
    /// <summary>
    ///     Registers all repository services using generic read interfaces in the Dependency Injection container.
    /// </summary>
    /// <param name="services">The service collection to add the repository services to.</param>
    /// <returns>The updated <see cref="IServiceCollection" />.</returns>
    public static IServiceCollection AddRepositoryServices(this IServiceCollection services)
    {
        // The flat ones <3
        services.AddScoped<IListItemsRepository<DriverLicenseEntity, DriverLicenseComplex>, DriverLicenseRepository>();
        services.AddScoped<IListItemsRepository<HorseSexEntity, HorseSexComplex>, HorseSexRepository>();
        services.AddScoped<IListItemsRepository<HorseTypeEntity, HorseTypeComplex>, HorseTypeRepository>();
        services.AddScoped<IListItemsRepository<RaceCartTypeEntity, RaceCartTypeComplex>, RaceCartTypeRepository>();
        services.AddScoped<IListItemsRepository<RaceCourseEntity, RaceCourseComplex>, RaceCourseRepository>();
        services
            .AddScoped<IListItemsRepository<RaceGamblingTypeEntity, RaceGamblingTypeComplex>,
                RaceGamblingTypeRepository>();
        services.AddScoped<IListItemsRepository<RaceStartTypeEntity, RaceStartTypeComplex>, RaceStartTypeRepository>();

        // The sourced ones ^^
        services.AddScoped<ISourceItemRepository<DriverEntity, DriverComplex>, DriverRepository>();
        services.AddScoped<ISourceItemRepository<HorseEntity, HorseComplex>, HorseRepository>();

        // the advanced repositories ;)
        services.AddScoped<IListItemsRepository<CompetitionEntity, CompetitionComplex>, CompetitionRepository>();

        return services;
    }
}