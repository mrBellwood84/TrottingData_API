using Models.Complex;
using Models.Simple;
using Persistence.Implementations;
using Persistence.Interfaces;

namespace API.Extensions;

public static class PersistenceExtensions
{
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