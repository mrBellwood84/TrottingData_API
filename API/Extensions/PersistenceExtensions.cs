using Models.Complex;
using Models.Entity;
using Persistence.Implementations;
using Persistence.Interfaces;

namespace API.Extensions;

/// <summary>
/// Extension methods for configuring persistence and database services.
/// </summary>
public static class PersistenceExtensions
{
    /// <summary>
    /// Registers all database services (IDbService) in the Dependency Injection container.
    /// </summary>
    public static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        // the simple data models <3
        services.AddScoped<IDbService<DriverLicenseEntity, DriverLicenseComplex>, DriverLicenseDbService>();
        services.AddScoped<IDbService<HorseSexEntity, HorseSexComplex>, HorseSexDbService>();
        services.AddScoped<IDbService<HorseTypeEntity, HorseTypeComplex>, HorseTypeDbService>();
        services.AddScoped<IDbService<RaceCartTypeEntity, RaceCartTypeComplex>, RaceCartTypeDbService>();
        services.AddScoped<IDbService<RaceCourseEntity, RaceCourseComplex>, RaceCourseDbService>();
        services.AddScoped<IDbService<RaceGamblingTypeEntity, RaceGamblingTypeComplex>, RaceGamblingTypeDbService>();
        services.AddScoped<IDbService<RaceStartTypeEntity, RaceStartTypeComplex>, RaceStartTypeDbService>();

        return services;
    }
}