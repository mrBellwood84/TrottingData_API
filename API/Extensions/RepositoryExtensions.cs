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
        services.AddScoped<IReadAllRepository<DriverLicenseEntity, DriverLicenseComplex>, DriverLicenseRepository>();
        services.AddScoped<IReadAllRepository<HorseSexEntity, HorseSexComplex>, HorseSexRepository>();
        services.AddScoped<IReadAllRepository<HorseTypeEntity, HorseTypeComplex>, HorseTypeRepository>();
        services.AddScoped<IReadAllRepository<RaceCartTypeEntity, RaceCartTypeComplex>, RaceCartTypeRepository>();
        services.AddScoped<IReadAllRepository<RaceCourseEntity, RaceCourseComplex>, RaceCourseRepository>();
        services
            .AddScoped<IReadAllRepository<RaceGamblingTypeEntity, RaceGamblingTypeComplex>,
                RaceGamblingTypeRepository>();
        services.AddScoped<IReadAllRepository<RaceStartTypeEntity, RaceStartTypeComplex>, RaceStartTypeRepository>();

        // The sourced ones ^^
        services.AddScoped<IReadSourceRepository<DriverEntity, DriverComplex>, DriverRepository>();
        services.AddScoped<IReadSourceRepository<HorseEntity, HorseComplex>, HorseRepository>();

        // the advanced repositories ;)
        services.AddScoped<IReadAllRepository<CompetitionEntity, CompetitionComplex>, CompetitionRepository>();
        services.AddScoped<IRaceRepository, RaceRepository>();
        services.AddScoped<IRaceResultRepository, RaceResultRepository>();

        return services;
    }
}