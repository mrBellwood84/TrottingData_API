using Models.Complex;
using Models.Entity;
using Persistence.Implementations;
using Persistence.Interfaces;

namespace API.Extensions;

/// <summary>
///     Extension methods for configuring persistence and database services.
/// </summary>
public static class PersistenceExtensions
{
    /// <summary>
    ///     Registers all database services in the Dependency Injection container.
    /// </summary>
    public static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        // Bulk read and lookup services
        services.AddScoped<IReadAllDbService<DriverLicenseEntity, DriverLicenseComplex>, DriverLicenseDbService>();
        services.AddScoped<IReadAllDbService<HorseSexEntity, HorseSexComplex>, HorseSexDbService>();
        services.AddScoped<IReadAllDbService<HorseTypeEntity, HorseTypeComplex>, HorseTypeDbService>();
        services.AddScoped<IReadAllDbService<RaceCartTypeEntity, RaceCartTypeComplex>, RaceCartTypeDbService>();
        services.AddScoped<IReadAllDbService<RaceCourseEntity, RaceCourseComplex>, RaceCourseDbService>();
        services
            .AddScoped<IReadAllDbService<RaceGamblingTypeEntity, RaceGamblingTypeComplex>, RaceGamblingTypeDbService>();
        services.AddScoped<IReadAllDbService<RaceStartTypeEntity, RaceStartTypeComplex>, RaceStartTypeDbService>();

        // Sourced services supporting external identifier lookups
        services.AddScoped<IReadSourcedDbService<DriverEntity, DriverComplex>, DriverDbService>();
        services.AddScoped<IReadSourcedDbService<HorseEntity, HorseComplex>, HorseDbService>();

        // The advanced ones  ^^
        services.AddScoped<IReadAllDbService<CompetitionEntity, CompetitionComplex>, CompetitionDbService>();
        services.AddScoped<IRaceDbService, RaceDbService>();
        services.AddScoped<IRaceParticipantDbService, RaceParticipantDbService>();
        services.AddScoped<IRaceResultDbService, RaceResultDbService>();

        return services;
    }
}